using System;

class Node
{
    public int Data;
    public Node? Next; // Nullable
    public Node(int data) { Data = data; Next = null; }
}

class Queue
{
    private Node? front; // Nullable
    private Node? rear;  // Nullable

    public void Insert(int data)
    {
        Node newNode = new Node(data);
        if (rear == null)
        {
            front = rear = newNode;
        }
        else
        {
            rear.Next = newNode;
            rear = newNode;
        }
    }

    public void Delete()
    {
        if (front == null)
        {
            Console.WriteLine("Queue is empty");
            return;
        }
        front = front.Next;
        if (front == null) rear = null;
    }

    public void DisplayFrontRear()
    {
        Console.WriteLine($"Front: {(front != null ? front.Data : -1)}");
        Console.WriteLine($"Rear: {(rear != null ? rear.Data : -1)}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Queue q = new Queue();

        q.Insert(10);
        q.Insert(20);
        q.Insert(30);

        q.DisplayFrontRear(); // Should print Front: 10, Rear: 30

        q.Delete();
        q.DisplayFrontRear(); // Should print Front: 20, Rear: 30
    }
}
