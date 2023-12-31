﻿namespace AdventOfCode2023
{
    public class Day07Puzzle01 : IPuzzle
    {
        enum HandType
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind,
        };

        private class HandData
        {
            public string Hand { get; set; }
            public HandType HandType { get; set; }
            public int Weight { get; set; }
            public int Bid { get; set; }
        }

        public void Initialize()
        {
            string[] puzzleInput = ReadFile(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Inputs\inputDay07.txt")));

            Console.WriteLine($"Puzzle 01 Result: {CalculateAnswer(puzzleInput)}");
        }

        private static string[] ReadFile(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        private static long CalculateAnswer(string[] fileData)
        {
            List<HandData> hands = new();
            long totalValue = 0;

            foreach (string line in fileData)
            {
                // Deconstructing the line so we get individual data
                string[] parts = line.Split(' ');
                string handPart = parts[0];
                int bidPart = int.Parse(parts[1]);

                HandType handTypePart = GetHandType(handPart);
                int weightPart = CalculateWeight(handPart, "23456789TJQKA");

                hands.Add(new HandData
                {
                    Hand = handPart,
                    HandType = handTypePart,
                    Weight = weightPart,
                    Bid = bidPart
                });
            }

            IOrderedEnumerable<HandData> orderedHands = hands.OrderBy(x => x.HandType).ThenBy(x => x.Weight);

            int i = 0;
            foreach (HandData hand in orderedHands)
            {
                totalValue += hand.Bid * (i + 1);
                i++;
            }

            return totalValue;
        }

        private static HandType GetHandType(string hand)
        {
            Dictionary<HandType, int> cardCounts = new();

            foreach (HandType card in hand)
            {
                if (cardCounts.ContainsKey(card))
                {
                    cardCounts[card]++;
                }
                else
                {
                    cardCounts.Add(card, 1);
                }
            }

            int[] counts = cardCounts.Values.OrderByDescending(count => count).Append(0).ToArray();

            return DetermineHandType(counts);
        }

        private static HandType DetermineHandType(int[] groups)
        {
            // AI to determine the current hand type
            if (groups.Length >= 1 && groups[0] == 5)
            {
                return HandType.FiveOfAKind;
            }
            else if (groups.Length >= 1 && groups[0] == 4)
            {
                return HandType.FourOfAKind;
            }
            else if (groups.Length >= 2 && groups[0] == 3 && groups[1] == 2)
            {
                return HandType.FullHouse;
            }
            else if (groups.Length >= 1 && groups[0] == 3)
            {
                return HandType.ThreeOfAKind;
            }
            else if (groups.Length >= 2 && groups[0] == 2 && groups[1] == 2)
            {
                return HandType.TwoPair;
            }
            else if (groups.Length >= 1 && groups[0] == 2)
            {
                return HandType.OnePair;
            }
            else
            {
                return HandType.HighCard;
            }
        }

        private static int CalculateWeight(string hand, string cardOrder)
        {
            int totalWeight = 0;

            for (int index = 0; index < hand.Length; index++)
            {
                char card = hand[index];
                int cardPosition = cardOrder.IndexOf(card);
                int shiftedWeight = cardPosition << (4 * (5 - index));

                totalWeight += shiftedWeight;
            }

            return totalWeight;
        }
    }
}
