﻿using System;
using System.Collections.Generic;
using System.Linq;
using Opdracht1;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Rogue
{
    class Turn
    {
        private readonly Game game;
        private readonly Recorder recorder;
        private readonly IInputReader inputReader;
        public bool packRetreated;

        public Turn(Game game, Recorder recorder, IInputReader inputReader)
        {
            this.inputReader = inputReader;
            this.recorder = recorder;
            this.game = game;
        }

        public void exec()
        {
            this.writeStatus();
            if (this.inCombat()) {
                this.doCombat();
            } else if (this.checkNode()) {
                this.playerTurn();
                this.packTurn();    
            }
        }

        private bool inCombat()
        {
            return this.getPlayer().node.doCombat(this.getPlayer());
        }

        public void playerTurn()
        {
            Console.WriteLine("What action do you want to do?");
            string input = this.inputReader.readInput();

            if (this.recorder != null) {
                if (input == "record start") {
                    this.recorder.start(this.game.state);
                    this.playerTurn();
                    return;
                }

                if (input == "record stop") {
                    this.recorder.stop();
                    this.playerTurn();
                    return;
                }
            }
            
            string[] temp = input.ToLower().Split();
            if (temp.Length < 2) {
                Console.WriteLine("Action is not valid, try another command");
                this.playerTurn();
                return;
            }

            string command = temp[0];
            string argument = temp[1];

            Player player = this.getPlayer();
            int newNodeNumber;
            if (command == "move" && int.TryParse(argument, out newNodeNumber)) {
                if (player.tryMove(newNodeNumber)) return;
                this.playerTurn();
                return;
            }

            if (command == "use-potion") {
                if (argument == "healingpotion" && this.useHealingPotion()) {
                    return;
                }
                if (argument == "timecrystal" && this.useTimeCrystal(false)) {
                    return;
                }

                Console.WriteLine("I can't drink a " + argument + ", try again");
                this.playerTurn();
                return;
            }

            Console.WriteLine("Action is not valid, try another command");
            this.playerTurn();
        }

        private bool useTimeCrystal(bool inBattle)
        {
            Player player = this.getPlayer();
            Node node = player.node;
            TimeCrystal timeCrystal = player.getTimeCrystal();

            if (timeCrystal == null) {
                Console.WriteLine("You have no timecrystal, try another command");
                return false;
            }

            if (inBattle) {
                player.useTimeCrystal(timeCrystal, true);
                return true;
            }

            if (node.zone != null && node.isEndNode()) {
                player.useTimeCrystal(timeCrystal, false);
                return true;
            }

            Console.WriteLine("You're neither in combat nor on a bridge, try doing something else!");
            return false;
        }

        private bool useHealingPotion()
        {
            Player player = this.getPlayer();
            List<HealingPotion> healingPotions = player.getHealingPotions();
            if (!healingPotions.Any()) {
                Console.WriteLine("You have no healingpotions, try another command");
                return false;
            }

            Console.WriteLine("Which healingpotion do you want to use?");
            for (int i = 0; i < healingPotions.Count; i++) {
                Console.WriteLine(i + ". " + "hp: " + healingPotions[i].hitPoints);
            }

            int number = int.Parse(this.inputReader.readInput());
            while (0 <= number || number < healingPotions.Count) {
                number = int.Parse(this.inputReader.readInput());
            }

            player.useHealingPotion(healingPotions[number]);

            return true;
        }

        private void doCombat()
        {
            Player player = this.getPlayer();
            Node node = player.node;
            Pack pack = node.packs.First();

            Console.WriteLine("Combat has begon");
            player.numberOfCombatsOfDungeon++;
            while (
                pack.monsters.Any() && 
                player.hitPoints >= 0 && 
                !node.stopCombat && 
                !packRetreated
            )
            {
                this.doCombatRound(player, pack);
            }
            
            if(!pack.monsters.Any()) {
                Console.WriteLine("Pack is dead");
                node.packs.Remove(pack);
            }

            if(player.hitPoints <= 0) {
                player.isAlive = false;
                this.game.endOfGame();
            }

            if(node.stopCombat) {
                this.retreatPlayer();
                node.stopCombat = false;
                player.timeCrystalActive = false;
            }

            if (packRetreated) {
                this.retreatPack(pack);
                Console.WriteLine("pack retreated");
                packRetreated = false;
                player.timeCrystalActive = false;
            }
        }

        private void doCombatRound(Player player, Pack pack)
        {
            this.printCombatStatus(player, pack);

            string input = this.inputReader.readInput();
            while (
                input != "continue" && 
                input != "retreat" && 
                input != "timecrystal"
            ){
                Console.WriteLine("This is not an option!");
                input = this.inputReader.readInput();
            }

            if (input == "continue"){
                player.attack(pack.monsters[0]);
                int totalHP = 0;
                foreach(Monster monster in pack.monsters)
                {
                    totalHP += monster.hitPoints;
                }
                if(totalHP > 0 && totalHP < player.hitPoints)
                {
                    packRetreated = true;
                }
                else
                {
                    pack.attack(player);
                }
                
                return;
            }

            if (input == "retreat") {
                player.node.stopCombat = true;
                return;
            }

            if (input == "timecrystal"){
                this.useTimeCrystal(true);
            }
        }

         private void retreatPlayer()
         {
            Player player = this.getPlayer();
            List<Node> neighbours = player.node.neighbours;
            Console.Write("To which node do you want to go: ");
            bool first = true;

            foreach (Node neighbour in neighbours)
            {
                if (first)
                    Console.Write(neighbour.number);
                else
                    Console.Write(", " + neighbour.number);
                first = false;
            }
            Console.WriteLine("?");
             
            int output;
            string[] temp = this.inputReader.readInput().Split();
            while (temp.Length != 1 || (!int.TryParse(temp[0], out output)) || !neighbours.Exists(item => item.number == int.Parse(temp[0])))
            {
                Console.WriteLine("Action is not valid, try another command");
                temp = this.inputReader.readInput().Split();
            }
            
            Node node = neighbours.Find(item => item.number == int.Parse(temp[0]));
            player.move(node);
            
        }

        public void retreatPack(Pack pack)
        {
            List<Node> neighbours = new List<Node>(pack.node.neighbours);
            for(int i = 0; i < neighbours.Count();i++)
            {
                if (neighbours[i].zone != pack.node.zone)
                {
                    neighbours.Remove(neighbours[i]);
                    i--;
                }
                    
                
            }
            Random random = new Random(90);
            int index = random.Next(0, neighbours.Count() - 1);
            pack.move(neighbours[index]);
        }

        private void printCombatStatus(Player player, Pack pack)
        {
            Console.Write("Your health is: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(player.hitPoints);
            Console.ResetColor();
            int totalHealth = 0;
            foreach (Monster monster in pack.monsters)
                totalHealth += monster.hitPoints;
            Console.Write("Enemy has ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(pack.monsters.Count);
            Console.ResetColor();
            Console.Write(" monsters left with a total health of: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(totalHealth);
            Console.ResetColor();

            Console.WriteLine(player.bag.Exists(item => item.GetType() == typeof(TimeCrystal))
                ? "retreat or continue the combat? Or use a TimeCrystal?"
                : "retreat or continue the combat?");
        }

        private Player getPlayer()
        {
            return this.game.state.player;
        }

        private void writeStatus()
        {
            Player player = this.getPlayer();

            Console.WriteLine("You're in node: " + player.node.number);
            Console.Write("Your HP: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(player.hitPoints);
            Console.ResetColor();
            Console.Write(" KP: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(player.killPoints);
            Console.ResetColor();
            Console.Write("You've got in your bag: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (player.bag.Count == 0)
                Console.Write("empty");
            foreach (Item item in player.bag) {
                Console.Write(item.getItemType() + ", ");
            }
            Console.ResetColor();
            Console.WriteLine();

            List<Node> neighbours = player.node.neighbours;
            Console.Write("Your neighbours are: ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            bool first = true;
            foreach (Node neighbour in neighbours) {
                if (first)
                    Console.Write(neighbour.number);
                else
                    Console.Write(", " + neighbour.number);
                first = false;
            }
            Console.ResetColor();
            Console.WriteLine();

        }

        public void packTurn()
        {
            Dungeon dungeon = this.getDungeon();
            Player player = this.getPlayer();

            foreach (Zone zone in dungeon.zones) {
                foreach (Node node in zone.nodes) {
                   for (int i = 0; i < node.packs.Count; i++)
                    {
                        
                        Pack pack = node.packs[i];
                        if (pack.isMoved)
                            continue;

                        List<Node> nodes = pack.node.neighbours;
                        if (nodes.Any())
                        {
                            if (zone == dungeon.zones[dungeon.zones.Count - 1])
                            {
                                if (player.node.zone == null || player.node.zone.number == zone.dungeon.zones.Count - 2)
                                {
                                    this.moveTowardsShortestPath(pack);
                                }
                                else if(player.node.zone == zone)
                                {
                                    this.chasePlayer(pack);
                                }
                            }
                            else
                            {
                                this.movePack(pack);
                            }
                        }
                        pack.isMoved = true;
                    }
                    foreach (Pack pack in node.packs)
                        pack.isMoved = false;



                }
            }
        }

        private Dungeon getDungeon()
        {
            return this.game.state.dungeon;
        }

        public void chasePlayer(Pack pack)
        {
            List<Node> nodesToPlayer = this.getNodesWithShortestPath(pack.node, this.getPlayer().node);
            if(nodesToPlayer.Count > 1 && nodesToPlayer[1].zone == pack.node.zone)
                pack.move(nodesToPlayer[1]);
        }

        public void moveTowardsShortestPath(Pack pack)
        {
            Zone zone = pack.node.zone;
            List<Node> nodesInPath = this.getNodesWithShortestPath(zone.startNode, zone.endNode);

            if (!nodesInPath.Contains(pack.node))
            {
                List<Node> shortest = null;
                foreach (Node node in nodesInPath)
                {
                    List<Node> nodes = this.getNodesWithShortestPath(pack.node, node);
                    if (shortest == null || nodes.Count < shortest.Count)
                    {
                        shortest = nodes;
                    }
                }
                if(shortest[0].zone == zone)
                    pack.move(shortest[0]);
            }
        }

        public void movePack(Pack pack)
        {
            Player player = this.getPlayer();
            Zone zone = pack.node.zone;

            List<Node> nodesToPlayer = this.getNodesWithShortestPath(pack.node, player.node);
            List<Node> nodesToEndNode = this.getNodesWithShortestPath(pack.node, zone.endNode);
            int M = 3;
            int maxMonstersInNode = M * (game.state.dungeon.level + 1);
            int count = 0;

            if (nodesToEndNode.Count > nodesToPlayer.Count && pack.node != this.game.state.player.node && nodesToPlayer[1].zone == zone)
            {
                foreach (Pack pack_in_node in nodesToPlayer[1].packs)
                {
                    count += pack_in_node.monsters.Count;
                }
                if (count + pack.monsters.Count <= maxMonstersInNode)
                    pack.move(nodesToPlayer[1]);
            }
            else if (pack.node != zone.endNode && nodesToEndNode[1].zone == zone)
            {
                
                foreach(Pack pack_in_node in nodesToEndNode[1].packs)
                {
                    count += pack_in_node.monsters.Count;
                }
                if(count + pack.monsters.Count <= maxMonstersInNode)
                    pack.move(nodesToEndNode[1]);

            }
               
        }

   

        public bool checkNode()
        {
            Dungeon dungeon = this.getDungeon();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            if (this.game.state.player.node.isExitNode()) {
                Console.WriteLine(
                    "This is the exit node of zone:" + 
                    this.getPlayer().node.zone.number +
                    " (end of dungeon with level: " + dungeon.level + ")");
                this.game.nextDungeon();
                dungeon = this.game.state.dungeon;
                Console.ResetColor();
                return false;
            }

            if (this.game.state.player.node.isEndNode()) {
                Console.WriteLine("This is the end node (bridge) of zone: " + this.getPlayer().node.zone.number + " in dungeon with level: " + dungeon.level);
            }
            Console.ResetColor();

            return true;
        }

        public List<Node> getNodesWithShortestPath(Node startNode, Node endNode)
        {
            Queue<List<Node>> queue = new Queue<List<Node>>();
            List<Node> nodeList = new List<Node>();
            nodeList.Add(startNode);
            queue.Enqueue(nodeList);
            while(queue.Count > 0)
            {
                List<Node> current = queue.Dequeue();
                if(current.Last() == endNode)
                {
                    return current;
                }
                List<Node> neighbours = current.Last().neighbours;
                foreach(Node neighbour in neighbours)
                {
                    List<Node> nodes = new List<Node>(current);
                    nodes.Add(neighbour);
                    queue.Enqueue(nodes);
                }
            }
            return null;
        }
    }
}
