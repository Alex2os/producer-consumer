using System;

namespace ProducerConsumer
{
    public partial class LinkedList
    {
        private Vertex ?head; // we add the ? so the compiler doesn't give warnings. it's just a symbol for nulls.
        private Vertex ?tail;
        int actual_consumer_vertex = 0;
        int actual_producer_vertex = 0;
        int list_size = 0;

        public LinkedList()
        {
            head = null;
            tail = null;
        }

        public void AssignVertexValue(string _value)
        {
            Vertex ?temp_node = head;

            for (int i = 0; i < list_size; i++)
            {
                if (i == actual_producer_vertex)
                {
                    temp_node.value = _value;
                    return;
                }

                temp_node = temp_node.next;
            }
        }

        public void DeAssignVertexValue()
        {
            Vertex? temp_node = head;
            for (int i = 0; i < list_size; i++)
            {
                if (i == actual_consumer_vertex)
                {
                    temp_node.value = "";
                    return;
                }

                temp_node = temp_node.next;
            }
        }

        public void PrintList()
        {
            Vertex? temp_node = head;
            for (int i = 0; i < list_size; i++)
            {
                Console.WriteLine(temp_node.value);
                temp_node = temp_node.next;
            }
        }

        public void AppendVertex(string _value)
        {
            Vertex? temp_node = new Vertex(_value);
            if (list_size == 0)
            {
                head = temp_node;
                tail = temp_node;
            }
            else
            {
                tail.next = temp_node;
                tail = temp_node;
            }

            list_size++;

            if (list_size == 21) // we make a circular linkedlist once we hit the 21 size.
            {
                tail.next = head;
            }
        }
    }

    public partial class Vertex
    {
        public string ?value;
        public Vertex ?next;

        public Vertex(string _value)
        {
            value = _value;
        }
    }

}