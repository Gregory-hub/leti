using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace work2
{
    internal class Node
    {
        public Point Coords;
        public int Width;
        public Node[] Children = null;
        public static int ChildrenCount = 5;

        public Node Father;
        public Point TreeNodeCoords;

        public Node(Point coords, int width, Node father = null)
        {
            Coords = coords;
            Width = width;
            Father = father;
        }
    }

    internal class Tree
    {
        public int MaxLevel = 5;
        public Node Root = null;

        public void Build(int width)
        {
            Root = new Node(new Point(0, 0), width);
            BuildChildren(Root, 0);
        }

        public void BuildChildren(Node node, int level)
        {
            if (level == MaxLevel) return;

            node.Children = new Node[Node.ChildrenCount];
            int width = node.Width / 3;
            node.Children[0] = new Node(node.Coords, width, node);
            node.Children[1] = new Node(new Point(node.Coords.X + 2 * width, node.Coords.Y), width, node);
            node.Children[2] = new Node(new Point(node.Coords.X + width, node.Coords.Y + width), width, node);
            node.Children[3] = new Node(new Point(node.Coords.X, node.Coords.Y + 2 * width), width, node);
            node.Children[4] = new Node(new Point(node.Coords.X + 2 * width, node.Coords.Y + 2 * width), width, node);

            BuildChildren(node.Children[0], level + 1);
            BuildChildren(node.Children[1], level + 1);
            BuildChildren(node.Children[2], level + 1);
            BuildChildren(node.Children[3], level + 1);
            BuildChildren(node.Children[4], level + 1);
        }

        public void GetLevelNodes(Node node, int target_level, int current_level, ref List<Node> nodes)
        {
            if (current_level == target_level)
                nodes.Add(node);
            else
                foreach (Node n in node.Children)
                    GetLevelNodes(n, target_level, current_level + 1, ref nodes);
        }
    }
}

