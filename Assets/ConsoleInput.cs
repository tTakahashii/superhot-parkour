using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsoleCommand
{
    public string commandName { get; set; }
    public string commandDescription { get; set; }
    public Action commandFunction { get; set; }
    public Action<string[]> commandFunctionArgumentless { get; set; }

    public ConsoleCommand(string name, string description, Action function = null, Action<string[]> argumentlessFunction = null)
    {
        commandName = name;
        commandDescription = description;
        commandFunction = function;
        commandFunctionArgumentless = argumentlessFunction;
    }

    public void ExecuteFunction()
    {
        commandFunction();
    }

    public void ExecuteFunction(string[] args)
    {
        commandFunctionArgumentless(args);
    }
}

public class ConsoleInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private ScrollRect consoleScroll;
    [SerializeField] private TMP_Text consoleText;
    //private static TextMeshProUGUI staticText;

    private CommandFunctions commandFunctions;
    private ConsoleCommand help, clear;

    private string inputText = "";

    private List<ConsoleCommand> commands;


    void Awake()
    {
        commandFunctions = new CommandFunctions(consoleText, null);

        #region COMMAND ASSIGNMENTS

        help = new ConsoleCommand("help",
        "Welcome to the Developer Console! To see the list of commands, type 'commands'. To see additional info about a command, type 'help (command name)'.",
        commandFunctions.Help,
        commandFunctions.Help);

        clear = new ConsoleCommand("clear",
        "Clears the console, removes all the messages.",
        commandFunctions.Clear);

        #endregion

        commands = new List<ConsoleCommand>() { help, clear };
        commandFunctions.commands = commands;

        var inputSubmit = new TMP_InputField.SubmitEvent();
        inputSubmit.AddListener(GetInputString);
        inputField.onEndEdit = inputSubmit;
    }

    private void OnEnable()
    {
        inputField.ActivateInputField();
    }

    // Update is called once per frame
    void GetInputString(string command)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            inputText = command;
            inputField.text = "";
            inputField.ActivateInputField(); 
        }   
    }

    private void Update()
    {
        if (inputText != "")
        {
            string[] command = inputText.ToLower().Split(" ");
            string commandName = command[0];

            //Debug.Log("COMMAND NAME: " + commandName);

            consoleText.text += "\n";

            if (command.Length > 1)
            {
                ArraySegment<string> commandArguments = new ArraySegment<string>(command, 1, command.Length - 1);

                //if (commands.TryGetValue(command[0], out Action<string[]> commandFunction))
                //{
                //    commandFunction(commandArguments.ToArray());
                //}
                //else
                //{
                //    Debug.LogError("Invalid command! Type 'help' or 'commands' to learn more.");
                //}

                ConsoleCommand currentCommand = commandFunctions.SearchCommands(commandName);

                if (currentCommand != null)
                {
                    currentCommand.ExecuteFunction(commandArguments.ToArray());
                }
                else
                {
                    commandFunctions.ReturnUnknownError();
                }
            }
            else
            {


                //if (argumentlessCommands.TryGetValue(command[0], out System.Action commandFunction))
                //{
                //    commandFunction();
                //}
                //else
                //{
                //    Debug.LogError("Invalid command! Type 'help' or 'commands' to learn more.");
                //}

                ConsoleCommand currentCommand = commandFunctions.SearchCommands(commandName);

                if (currentCommand != null)
                {
                    currentCommand.ExecuteFunction();
                }
                else
                {
                    commandFunctions.ReturnUnknownError();
                }
            }

            consoleText.text += "\n ";

            inputText = "";
            consoleScroll.verticalNormalizedPosition = 0f;
        }
        consoleScroll.verticalNormalizedPosition = 0f;
    }

    public class CommandFunctions
    {
        private TMP_Text consoleText;
        public List<ConsoleCommand> commands;

        public enum ErrorCodes { HELP_COMMAND_NOT_FOUND, DEFAULT_ERROR }

        public CommandFunctions(TMP_Text displayText, List<ConsoleCommand> commandList)
        {
            consoleText = displayText;
            commands = commandList;
        }

        public ConsoleCommand SearchCommands(string commandName)
        {
            foreach (ConsoleCommand command in commands)
            {
                if (command != null)
                {
                    if (command.commandName == commandName)
                    {
                        return command;
                    }
                }           
            }

            //Debug.Log("returning null");
            return null;
        }

        public void ReturnUnknownError(ErrorCodes errorCode = ErrorCodes.DEFAULT_ERROR)
        {
            consoleText.text += "<color=\"red\">";

            switch (errorCode)
            {
                case ErrorCodes.HELP_COMMAND_NOT_FOUND:
                    consoleText.text += "The command you requested info about couldn't be found. Type 'commands' to get a list of all commands.";
                    break;
                default:
                    consoleText.text += "Unknown command! Type 'commands' to get a list of all commands.";
                    break;
            }

            consoleText.text += "</color>";
        }

        public void Help()
        {
            consoleText.text += "Welcome to the Developer Console! To see the list of commands, type 'commands'. To see additional info about a command, type 'help (command name)'.";
            //Debug.Log("Welcome to the Developer Console! To see the list of commands, type 'commands'. To see additional info about a command, type 'help (command name)'.");
        }

        public void Help(string[] additionalArguments)
        {
            ConsoleCommand commandRequested = SearchCommands(additionalArguments[0]);

            if (commandRequested != null)
            {
                consoleText.text += $"{commandRequested.commandDescription}";
            } else{
                ReturnUnknownError(ErrorCodes.HELP_COMMAND_NOT_FOUND);
            }

            
            //Debug.Log($"{SearchCommands(additionalArguments[0]).commandDescription}");
        }

        public void Clear()
        {
            consoleText.text = "";
        }
    }
}