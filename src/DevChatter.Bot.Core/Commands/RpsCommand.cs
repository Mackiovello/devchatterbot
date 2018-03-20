﻿using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Commands
{
    public class RockPaperScissorsCommand : SimpleCommand
    {
        private readonly IRepository _repository;

        private readonly List<(string username, RockPaperScissors choice)> _competitors = new List<(string, RockPaperScissors)>();

        public RockPaperScissorsCommand(IRepository repository)
        {
            _repository = repository;
            CommandText = "rps";
            RoleRequired = UserRole.Everyone;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string username = eventArgs?.ChatUser?.DisplayName;
            string argumentOne = eventArgs?.Arguments?.ElementAtOrDefault(0);
            if (argumentOne == "start" && username == _competitors.FirstOrDefault().username)
            {
                PlayMatch(chatClient);
            }
            else if (Enum.TryParse(argumentOne, true, out RockPaperScissors choice))
            {
                if (!_competitors.Any())
                {
                    StartNewMatch(chatClient, (username, choice));
                }
                else
                {
                    JoinMatch(chatClient, (username, choice));
                }

            }
            else
            {
                chatClient.SendMessage($"Please make a valid selection from this set: rock, paper, scissors");
            }
        }

        private void JoinMatch(IChatClient chatClient, (string username, RockPaperScissors choice) userChoice)
        {
            _competitors.Add(userChoice);
            chatClient.SendMessage($"{userChoice.username} joined in the Rock-Paper-Scissors game with {userChoice.choice}!");
        }

        private void StartNewMatch(IChatClient chatClient, (string username, RockPaperScissors choice) userChoice)
        {
            chatClient.SendMessage(
                $"{userChoice.username} wants to play Rock-Paper-Scissors! To join, simply type \"!rps\" in chat.");
            JoinMatch(chatClient, userChoice);
        }

        private void PlayMatch(IChatClient chatClient)
        {
            RockPaperScissors botChoice = GetRandomChoice();
            chatClient.SendMessage($"I choose {botChoice}!");
            AnnounceWinners(chatClient, botChoice);
            //AdjustTokens(botChoice);

            _competitors.Clear();
        }

        private void AnnounceWinners(IChatClient chatClient, RockPaperScissors botChoice)
        {
            var winningChoice = (RockPaperScissors) (((int) botChoice + 1) % 3);
            var winnersList = _competitors.Where(x => x.choice == winningChoice).Select(x => x.username);
            string winners = string.Join(",", winnersList);
            chatClient.SendMessage($"The winners are {winners}!");
        }

        private RockPaperScissors GetRandomChoice()
        {
            int randomNumber = MyRandom.RandomNumber(0, 3);
            return (RockPaperScissors) randomNumber;
        }
    }

    public enum RockPaperScissors
    {
        Rock,
        Paper,
        Scissors,
    }
}