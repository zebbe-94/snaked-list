public class SinglyLinkedList<T>
{
    public Node<T> head { private set; get; }
    public Node<T> tail { private set; get; }
    public int size { private set; get; }


    public class Node<T>
    {
        public T data;
        public Node<T> next;

        public Node(T data) 
        { 
            this.data = data;
            next = null;
        }
    }

    public SinglyLinkedList()
    {
        head = null; 
        tail = null;
        size = 0;
    }

    public void Add(T data)
    {
        if (head == null)
        {
            head = new Node<T>(data);
            tail = head;
        }
        else
        {
            tail.next = new Node<T>(data);
            tail = tail.next;
        }
        size++;
    }

    public void RemoveLast()
    {
        if (head == null)
        {
            return;
        }
        else if (head.next == null)
        {
            head = null;
        }
        else
        {
            Node<T> n = head;
            while(n.next != tail)
            {
                n = n.next;
            }
            n.next = null;
            tail = n;
        }
        size--;
    }
}
