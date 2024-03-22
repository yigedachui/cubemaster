using System.Collections.Generic;

public class CubeMatchContainer
{
    LinkedList<Cube> LinkedHead;
    public int CurrentIndex = 0;
    public const int MaxCount = 7;
    public delegate bool OnMatch(LinkedList<Cube> LinkedHead);
    public event OnMatch OnMatchEvent;

    public CubeMatchContainer()
    {
        LinkedHead = new LinkedList<Cube>();
    }

    public void AddCube(Cube cube)
    {
        SortCube(cube);
    }

    public void SortCube(Cube cube)
    {
        LinkedListNode<Cube> newNode = new LinkedListNode<Cube>(cube);

        LinkedListNode<Cube> listNode = LinkedHead.First;


        if (listNode == null)
        {
            LinkedHead.AddFirst(newNode);
            CurrentIndex++;
            return;
        }

        while (CurrentIndex < MaxCount)
        {
            if (listNode.Value.data.type == cube.data.type)
            {                
                while (listNode.Next != null)
                {
                    if (listNode.Next.Value.data.type == cube.data.type)
                    {
                        listNode = listNode.Next;
                        continue;
                    }
                    else
                    {
                        LinkedHead.AddAfter(listNode, newNode);
                        //LinkedHead.AddAfter(newNode, listNode.Next);
                        CurrentIndex++;
                        Matched();

                        return;
                    }
                    
                }
                LinkedHead.AddAfter(listNode, newNode);
                CurrentIndex++;
                break;
            }
            else
            {
                if (listNode.Next != null)
                {
                    listNode = listNode.Next;
                    continue;
                }
                else
                {
                    LinkedHead.AddAfter(listNode, newNode);
                    CurrentIndex++;
                    break;
                }
            }            
        }

        Matched();

    }

    public void Matched()
    {
        if (OnMatchEvent != null) 
        {
            if (OnMatchEvent(LinkedHead))
            {
                CurrentIndex -= 3;
            }
        }
    }

}
