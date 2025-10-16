using System;

namespace ProducerConsumer
{
    public partial class LinkedList
    {
        private Vertex ?head; // we add the ? so the compiler doesn't give warnings. it's just a symbol for nulls.
        private Vertex ?tail;
        public int actual_consumer_vertex = 0;
        public int actual_producer_vertex = 0;
        public int list_size = 0;

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

            // as same as the consumer, we update the producer vertex too when it produces.
            if (actual_producer_vertex + 1 == 22) actual_producer_vertex = 0;  
            else actual_producer_vertex++;
        }

        public void DeAssignVertexValue() // this function is for the consumer, when it is consuming a value.
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

            // every time the consumer eats, then we update the actual_consumer_vertex. also we check if the vertex numer is 22, so if it's that then we return to the head, which is vertex number zero in this case.
            if (actual_consumer_vertex + 1 == 22) actual_consumer_vertex = 0;  
            else actual_consumer_vertex++;
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

        public bool CheckConsumerVertex()
        {
            Vertex temp_node = head;
            for (int i = 0; i < list_size; i++)
            {
                if (i == actual_consumer_vertex)
                {
                    if (temp_node.value == "") return false;
                    else return true;
                }

                temp_node = temp_node.next;
            }

            return false;
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