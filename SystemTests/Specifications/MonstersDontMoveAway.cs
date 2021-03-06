﻿using System.Collections.Generic;
using System.Linq;
using Opdracht1;
using Rogue;
using Rogue.DomainObjects;
using System;
using System.Diagnostics;
using SystemTests.Specifications;

namespace SystemTests.Specifications
{
    class MonstersDontMoveAway : ISpecification
    {
        private Dictionary<Pack, int[]> packsDictionary;
        Game game;
        Zone currentZone;

        public MonstersDontMoveAway()
        {
        }

        public bool validate(Game game)
        {
            this.game = game;
            if (currentZone == null)
            {
                this.currentZone = game.state.dungeon.zones[0];
                this.initialize(currentZone);

            }
            else if(currentZone != game.state.player.node.zone)
            {
                if(game.state.player.node.zone == null)
                {
                    this.currentZone = game.state.dungeon.zones[0];
                }
                else currentZone = game.state.player.node.zone;
                this.initialize(this.currentZone);
                
            }

            bool temp = true;
            foreach (KeyValuePair<Pack, int[]> pair in this.packsDictionary) {
                List<Node> nodesToPlayer = shortestPath(pair.Key.node, game.state.player.node);
                List<Node> nodesToEndNode = shortestPath(pair.Key.node, currentZone.endNode);
                
                if (nodesToPlayer.Count() > pair.Value[0] && nodesToEndNode.Count() > pair.Value[1])
                {
                    temp = false;
                }
                   
            }
            return temp;
            
        }

        private void initialize(Zone zone)
        {
            this.packsDictionary = new Dictionary<Pack, int[]>();
                foreach (Node node in zone.nodes) {
                    foreach (Pack pack in node.packs) {
                        List<Node> nodesToPlayer = shortestPath(pack.node, game.state.player.node);
                        List<Node> nodesToEndNode = shortestPath(pack.node, zone.endNode);
                        int[] counts = new int[] { nodesToPlayer.Count(), nodesToEndNode.Count() };
                        this.packsDictionary.Add(pack, counts);
                    }
                }
            
        }

        public List<Node> shortestPath(Node startNode, Node endNode)
        {
            Queue<List<Node>> queue = new Queue<List<Node>>();
            List<Node> nodeList = new List<Node>();
            nodeList.Add(startNode);
            queue.Enqueue(nodeList);
            while (queue.Count > 0)
            {
                List<Node> current = queue.Dequeue();
                if (current.Last() == endNode)
                {
                    return current;
                }
                List<Node> neighbours = current.Last().neighbours;
                foreach (Node neighbour in neighbours)
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
