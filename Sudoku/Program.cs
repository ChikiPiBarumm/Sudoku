using System;
using System.Diagnostics;

int[,] board = new int[9, 9];
int n = 9; // number of columns/rows.
int SRN; // square root of N
int K = 0; // No. Of missing digits
int fails = 0; // number of fails
var timer = new Stopwatch();

double SRNd = Math.Sqrt(n);
SRN = (int)SRNd;

void Play()
{
    Welcome();
    Decision();
    DifficultyLevel();
    fillValues(n);
    Input();
    Output();
}

void Welcome()
{
    Console.WriteLine("Hello! Welcome to Sudoku game!");
    Console.WriteLine();
    Console.WriteLine("Made by KPI student Maksym Chernenko");
    Console.WriteLine();
    Console.Write("Do you want to play? (Y-yes/N-no) - ");

}

void Decision()
{
    string decision = Console.ReadLine();
    while (decision != "Y" && decision != "y")
    {
        if (decision == "N" || decision == "n")
        {
            Console.WriteLine();
            Console.WriteLine("Ohhh... So goodbye, have a good day!");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Enter valid response! Y=Yes / N=No");
            decision = Console.ReadLine();
        }
    }
}

int DifficultyLevel()
{
    Console.WriteLine();
    Console.Write("""
        Choose dificulty level:
        1) Easy
        2) Medium
        3) Hard
        4) Expert
        5) Set your own nubmer of missing digits
        Level: 
        """);
    string level = Console.ReadLine();
    while (level != "1"
        && level != "2"
        && level != "3"
        && level != "4"
        && level != "5")
    {
        Console.Write("Enter number from 1 to 5: ");
        level = Console.ReadLine();
    }
    if (level == "1")
    {
        K = 10;
    }
    if (level == "2")
    {
        K = 15;
    }
    if (level == "3")
    {
        K = 20;
    }
    if (level == "4")
    {
        K = 25;
    }
    if (level == "5")
    {
        Console.Write("Enter your own number of missing digits: ");
        K = Convert.ToInt16(Console.ReadLine());
    }
    return K;
}

void fillValues(int n)
{
    fillDiagonal();

    fillRemaining(0, SRN);

    removeKDigits();
}

void Input()
{
    printSudoku();
    while (!IsSolved())
    {
        timer.Start();
        Console.Write("Enter row (1-9): ");
        int row = int.Parse(Console.ReadLine()) - 1;
        Console.Write("Enter column (1-9): ");
        int col = int.Parse(Console.ReadLine()) - 1;
        Console.Write("Enter value (1-9): ");
        int value = int.Parse(Console.ReadLine());

        if (CheckIfSafe(row, col, value))
        {
            board[row, col] = value;
            printSudoku();
            Console.WriteLine("Correct number!");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Invalid move!");
            fails++;
            if (IsLost(fails))
            {
                Console.WriteLine();
                Console.WriteLine("You lost...");
                Environment.Exit(0);
            }
            Console.WriteLine("Fails {0}/3", fails);
            Console.WriteLine();
        }
    }
}

void Output()
{
    if (IsSolved() == true)
    {
        timer.Stop();
        TimeSpan timeTaken = timer.Elapsed;
        string time = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
        printSudoku();
        Console.WriteLine($"Congratulations!!! You have solved the sudoku! {time}");
        Environment.Exit(0);
    }
}

void fillDiagonal()
{
    for (int i = 0; i < n; i = i + SRN)
    {
        fillBox(i, i);
    }
}

bool fillRemaining(int i, int j)
{
    if (j >= n && i < n - 1)
    {
        i = i + 1;
        j = 0;
    }

    if (i >= n && j >= n)
    {
        return true;
    }

    if (i < SRN)
    {
        if (j < SRN)
        {
            j = SRN;
        }
    }
    else if (i < n - SRN)
    {
        if (j == (int)(i / SRN) * SRN)
        {
            j = j + SRN;
        }
    }
    else
    {
        if (j == n - SRN)
        {
            i = i + 1;
            j = 0;
            if (i >= n)
            {
                return true;
            }
        }
    }

    for (int num = 1; num <= n; num++)
    {
        if (CheckIfSafe(i, j, num))
        {
            board[i, j] = num;
            if (fillRemaining(i, j + 1))
            {
                return true;
            }
            board[i, j] = 0;
        }
    }
    return false;
}

void fillBox(int row, int col)
{
    int num;
    for (int i = 0; i < SRN; i++)
    {
        for (int j = 0; j < SRN; j++)
        {
            do
            {
                num = randomGenerator(n);
            }
            while (!unUsedInBox(row, col, num));

            board[row + i, col + j] = num;
        }
    }
}

void removeKDigits()
{
    int count = K;
    while (count != 0)
    {
        int cellId = randomGenerator(n * n) - 1;

        int i = (cellId / n);
        int j = cellId % 9;
        if (j != 0)
        {
            j = j - 1;

        }

        if (board[i, j] != 0)
        {
            count--;
            board[i, j] = 0;
        }
    }
}

int randomGenerator(int num)
{
    Random rand = new Random();
    return (int)Math.Floor((double)(rand.NextDouble() * num + 1));
}

bool CheckIfSafe(int i, int j, int num)
{
    return (unUsedInRow(i, num)
        && unUsedInCol(j, num)
        && unUsedInBox(i - i % SRN, j - j % SRN, num));
}

bool unUsedInBox(int rowStart, int colStart, int num)
{
    for (int i = 0; i < SRN; i++)
    {
        for (int j = 0; j < SRN; j++)
        {
            if (board[rowStart + i, colStart + j] == num)
            {
                return false;
            }
        }
    }
    return true;
}

bool unUsedInRow(int i, int num)
{
    for (int j = 0; j < n; j++)
    {
        if (board[i, j] == num)
        {
            return false;
        }
    }
    return true;
}

bool unUsedInCol(int j, int num)
{
    for (int i = 0; i < n; i++)
    {
        if (board[i, j] == num)
        {
            return false;

        }
    }
    return true;
}

void printSudoku()
{
    Console.WriteLine();
    for (int i = 0; i < 9; i++)
    {
        for (int j = 0; j < 9; j++)
        {
            Console.Write(board[i, j] + " ");
            if ((j + 1) % 3 == 0 && j < 8)
            {
                Console.Write("| ");
            }
        }
        Console.WriteLine();
        if ((i + 1) % 3 == 0 && i < 8)
        {
            Console.WriteLine("---------------------");
        }
    }
    Console.WriteLine();
}

bool IsLost(int fails)
{
    if (fails == 3)
    {
        return true;
    }
    return false;
}

bool IsSolved()
{
    for (int i = 0; i < 9; i++)
    {
        for (int j = 0; j < 9; j++)
        {
            if (board[i, j] == 0)
            {
                return false;
            }
        }
    }
    return true;
}

Play();
