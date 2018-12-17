using ProgramSedziowski.Exceptions;
using ProgramSedziowski.Model;
using ProgramSedziowski.Modules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProgramSedziowski
{
    public class ViewModel : DependencyObject, INotifyPropertyChanged
    {
        #region DependencyProperties

        public static readonly DependencyProperty ApplicationsPathProperty =
             DependencyProperty.Register(nameof(ApplicationsPath),// name of property 
             typeof(string),                                     // type of property
             typeof(ViewModel),
             new FrameworkPropertyMetadata(default(string)));    // default value of property
        public string ApplicationsPath                           // original .net property
        {
            get { return (string)GetValue(ApplicationsPathProperty); }
            set { SetValue(ApplicationsPathProperty, value); }
        }

        public static readonly DependencyProperty BoardSizeProperty =
             DependencyProperty.Register(nameof(BoardSize),
             typeof(int),
             typeof(ViewModel),
             new FrameworkPropertyMetadata(13));
        public int BoardSize
        {
            get { return (int)GetValue(BoardSizeProperty); }
            set { SetValue(BoardSizeProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty =
             DependencyProperty.Register(nameof(IsBusy),
             typeof(bool),
             typeof(ViewModel),
             new FrameworkPropertyMetadata(false));
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty CurrentGameNumberProperty =
             DependencyProperty.Register(nameof(CurrentGameNumber),
             typeof(int),
             typeof(ViewModel),
             new FrameworkPropertyMetadata(0));
        public int CurrentGameNumber
        {
            get { return (int)GetValue(CurrentGameNumberProperty); }
            set { SetValue(CurrentGameNumberProperty, value); }
        }

        public static readonly DependencyProperty TestingPercentageProperty =
             DependencyProperty.Register(nameof(TestingPercentage),
             typeof(string),
             typeof(ViewModel),
             new FrameworkPropertyMetadata("0%"));
        public string TestingPercentage
        {
            get { return (string)GetValue(TestingPercentageProperty); }
            set { SetValue(TestingPercentageProperty, value); }
        }

        public static readonly DependencyProperty GameApplicationListProperty =
             DependencyProperty.Register(nameof(GameApplicationList),
             typeof(ObservableCollection<GameApplication>),
             typeof(ViewModel),
             new FrameworkPropertyMetadata(new ObservableCollection<GameApplication>()));
        public ObservableCollection<GameApplication> GameApplicationList
        {
            get { return (ObservableCollection<GameApplication>)GetValue(GameApplicationListProperty); }
            set { SetValue(GameApplicationListProperty, value); }
        }

        public static readonly DependencyProperty ErrorsListProperty =
             DependencyProperty.Register(nameof(ErrorsList),
             typeof(ObservableCollection<Error>),
             typeof(ViewModel),
             new FrameworkPropertyMetadata(new ObservableCollection<Error>()));
        public ObservableCollection<Error> ErrorsList
        {
            get { return (ObservableCollection<Error>)GetValue(ErrorsListProperty); }
            set { SetValue(ErrorsListProperty, value); }
        }

        public static readonly DependencyProperty CurrentHistoryGameProperty =
             DependencyProperty.Register(nameof(CurrentHistoryGame),
             typeof(Game),
             typeof(ViewModel),
             new FrameworkPropertyMetadata(default(Game)));
        public Game CurrentHistoryGame
        {
            get { return (Game)GetValue(CurrentHistoryGameProperty); }
            set { SetValue(CurrentHistoryGameProperty, value); }
        }

        public static readonly DependencyProperty HistoryFilePathProperty =
             DependencyProperty.Register(nameof(HistoryFilePath),
             typeof(string),
             typeof(ViewModel),
             new FrameworkPropertyMetadata(default(string)));
        public string HistoryFilePath
        {
            get { return (string)GetValue(HistoryFilePathProperty); }
            set { SetValue(HistoryFilePathProperty, value); }
        }

        #endregion

        #region Commands

        public ICommand LoadGameApplicationsCommand { get; }

        public ICommand RunTestsCommand { get; }

        public ICommand SelectDirectoryCommand { get; }

        public ICommand SelectHistoryFileCommand { get; }

        public ICommand ResetCommand { get; }

        #endregion

        public List<Game> games = new List<Game>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            LoadGameApplicationsCommand = new CommandHandler(() => { LoadListOfPrograms(); });
            RunTestsCommand = new CommandHandler(() => { RunTestAllGameApplications(); });
            SelectDirectoryCommand = new CommandHandler(() => { SelectApplicationsFolder(); });
            SelectHistoryFileCommand = new CommandHandler(() => { SelectHistoryFile(); });
            ResetCommand = new CommandHandler(() => { ErrorsList.Clear(); GameApplicationList.ForEach((e) => { e.Result.DisNum = 0;e.Result.LosNum = 0;e.Result.WinNum = 0; }); });
        }

        private void SelectApplicationsFolder()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ApplicationsPath = dialog.SelectedPath;
                LoadListOfPrograms();
            }
        }

        private void SelectHistoryFile()
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Plik historii gry (*.gam)|*.gam";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                HistoryFilePath = dialog.FileName;
                CurrentHistoryGame = GameHistorySaverModule.Open(HistoryFilePath);
            }
        }

        public void LoadListOfPrograms()
        {
            GameApplicationList.Clear();
            ErrorsList.Clear();
            if (string.IsNullOrEmpty(ApplicationsPath))
            {
                throw new Exception("Podaj ścieżkę, gdzie znajdują się katalogi z grami!");
            }

            var dirInfo = new DirectoryInfo(ApplicationsPath);
            foreach (var dir in dirInfo.GetDirectories())
            {
                var infoFilePath = dir.FullName + @"\info.txt";
                if (File.Exists(infoFilePath))
                {
                    using (var fileReader = new StreamReader(infoFilePath, Encoding.UTF8))
                    {
                        try
                        {
                            var dataArray = fileReader.ReadToEnd().Replace("\r", "").Split('\n');
                            var commandLineSplit = dataArray[2].Split(new char[] { ' ' }, 2);
                            GameApplicationList.Add(new GameApplication()
                            {
                                Name = dataArray[0],
                                Author = dataArray[1],
                                CommandLine = (commandLineSplit.Length <= 1 ? "" : commandLineSplit[1]),
                                Path = (commandLineSplit.Length == 1 ? Path.Combine(dir.FullName, commandLineSplit[0]) : commandLineSplit[0]),
                                WorkingDirectory = dir.FullName
                            });
                        }
                        catch
                        {
                            ErrorsList.Add(new Error($"Nieprawidłowy format pliku {infoFilePath} !", infoFilePath));
                        }
                    }
                }
                else
                {
                    ErrorsList.Add(new Error($"Plik {infoFilePath} nie istnieje!", ""));
                }
            }

            GenerateGamePermutations();
        }

        public void GenerateGamePermutations()
        {
            games.Clear();
            foreach (var gameApplication1 in GameApplicationList)
            {
                foreach (var gameApplication2 in GameApplicationList)
                {
                    if (gameApplication1 != gameApplication2)
                    {
                        games.Add(new Game()
                        {
                            gamer1 = gameApplication1,
                            gamer2 = gameApplication2
                        });
                    }
                }
            }
        }

        private const int StaticFiledValue = 255;
        private const int CommandTimeout = 500;

        public void RunTestAllGameApplications()
        {
            CurrentGameNumber = 0;
            Task.Run(async () =>
            {
                Random randomGenerator = new Random(DateTime.Now.Millisecond);
                int boardSize = 13;
                Dispatcher.Invoke(() =>
                {
                    IsBusy = true;
                    boardSize = BoardSize;
                });

                foreach (var currentGame in games)
                {
                    Dispatcher.Invoke(() => { CurrentGameNumber++; TestingPercentage = ((double)CurrentGameNumber * 100 / games.Count).ToString("N0"); });

                    // create board for game
                    var boardArray = new int[boardSize, boardSize];
                    List<Model.Point> startPoints = new List<Model.Point>();
                    // fill 10% of board

                    for (int i = 0; i < 0.1 * boardSize * boardSize; i++)
                    {
                        int randX;
                        int randY;
                        do
                        {
                            randX = randomGenerator.Next(0, boardSize );
                            randY = randomGenerator.Next(0, boardSize);
                        } while (boardArray[randX, randY] == StaticFiledValue);

                        boardArray[randX, randY] = StaticFiledValue;
                        currentGame.moves.Add(new RegisteredMove(new Model.Point(randX, randY), StaticFiledValue));
                        startPoints.Add(new Model.Point(randX, randY));
                    }

                    // create processes
                    Process[] processes = new Process[2];
                    StreamWriter[] stdout = new StreamWriter[2];
                    StreamReader[] stdin = new StreamReader[2];

                    try
                    {
                        processes[0] = new Process();
                        processes[0].StartInfo.FileName = currentGame.gamer1.Path;
                        processes[0].StartInfo.Arguments = currentGame.gamer1.CommandLine;
                        processes[0].StartInfo.RedirectStandardInput = true;
                        processes[0].StartInfo.RedirectStandardOutput = true;
                        processes[0].StartInfo.UseShellExecute = false;
                        processes[0].StartInfo.CreateNoWindow = true;
                        processes[0].StartInfo.WorkingDirectory = currentGame.gamer1.WorkingDirectory;
                        processes[0].Start();

                        stdout[0] = processes[0].StandardInput;
                        stdin[0] = processes[0].StandardOutput;
                    }
                    catch (Exception ex)
                    {
                        DisqualificateGamer(currentGame, processes, 0, ex);
                        continue;
                    }

                    try
                    {
                        processes[1] = new Process();
                        processes[1].StartInfo.FileName = currentGame.gamer2.Path;
                        processes[1].StartInfo.Arguments = currentGame.gamer2.CommandLine;
                        processes[1].StartInfo.RedirectStandardInput = true;
                        processes[1].StartInfo.RedirectStandardOutput = true;
                        processes[1].StartInfo.UseShellExecute = false;
                        processes[1].StartInfo.CreateNoWindow = true;
                        processes[1].StartInfo.WorkingDirectory = currentGame.gamer2.WorkingDirectory;
                        processes[1].Start();

                        stdout[1] = processes[1].StandardInput;
                        stdin[1] = processes[1].StandardOutput;
                    }
                    catch (Exception ex)
                    {
                        DisqualificateGamer(currentGame, processes, 1, ex);
                        continue;
                    }

                    /* start */
                    try
                    {
                        stdout[0].WriteLine(boardSize.ToString());
                        var result = await stdin[0].ReadLineWithTimeout(CommandTimeout);
                        Debug.WriteLine(result);
                        if (result != "ok")
                        {
                            throw new InvalidAnswerException(result, "ok");
                        }
                    }
                    catch (Exception ex)
                    {
                        DisqualificateGamer(currentGame, processes, 0, ex);
                        continue;
                    }

                    try
                    {
                        stdout[1].WriteLine(boardSize.ToString());
                        var result = await stdin[1].ReadLineWithTimeout(CommandTimeout);
                        Debug.WriteLine(result);
                        if (result != "ok")
                        {
                            throw new InvalidAnswerException(result, "ok");
                        }
                    }
                    catch (Exception ex)
                    {
                        DisqualificateGamer(currentGame, processes, 1, ex);
                        continue;
                    }

                    /* Send start points */
                    try
                    {
                        stdout[0].WriteLine(ParsingFunctionsModule.PointsToString(startPoints));
                        var result = await stdin[0].ReadLineWithTimeout(CommandTimeout);
                        if (result != "ok")
                        {
                            throw new InvalidAnswerException(result, "ok");
                        }
                    }
                    catch (Exception ex)
                    {
                        DisqualificateGamer(currentGame, processes, 0, ex);
                        continue;
                    }

                    try
                    {
                        stdout[1].WriteLine(ParsingFunctionsModule.PointsToString(startPoints));
                        var result = await stdin[1].ReadLineWithTimeout(CommandTimeout);
                        if (result != "ok")
                        {
                            throw new InvalidAnswerException(result, "ok");
                        }
                    }
                    catch (Exception ex)
                    {
                        DisqualificateGamer(currentGame, processes, 1, ex);
                        continue;
                    }

                    // tutaj start bądź punkty przeciwnika
                    Model.Point[] previousPoints;
                    try
                    {
                        stdout[0].WriteLine("start");
                        previousPoints = ParsingFunctionsModule.AnswerStringToPoint(await stdin[0].ReadLineWithTimeout(CommandTimeout));
                        PositionCheckerModule.CheckPosition(previousPoints, boardSize, boardArray, 0, currentGame);
                    }
                    catch (Exception ex)
                    {
                        DisqualificateGamer(currentGame, processes, 0, ex);
                        continue;
                    }

                    try
                    {
                        stdout[1].WriteLine(ParsingFunctionsModule.PointsToString(previousPoints));
                        previousPoints = ParsingFunctionsModule.AnswerStringToPoint(await stdin[1].ReadLineWithTimeout(CommandTimeout));
                        PositionCheckerModule.CheckPosition(previousPoints, boardSize, boardArray, 1, currentGame);
                    }
                    catch (Exception ex)
                    {
                        DisqualificateGamer(currentGame, processes, 1, ex);
                        continue;
                    }

                    while (true)
                    {
                        try
                        {
                            stdout[0].WriteLine(ParsingFunctionsModule.PointsToString(previousPoints));
                            previousPoints = ParsingFunctionsModule.AnswerStringToPoint(await stdin[0].ReadLineWithTimeout(CommandTimeout));
                            PositionCheckerModule.CheckPosition(previousPoints, boardSize, boardArray, 0, currentGame);
                        }
                        catch (EmptyAnswerException)
                        {
                            if (PositionCheckerModule.IsAllBoardFull(boardArray, boardSize))
                            {
                                LoseGamer(currentGame, processes, 0);
                            }
                            else
                            {
                                DisqualificateGamer(currentGame, processes, 0, new EmptyAnswerException());
                            }

                            break;
                        }
                        catch (Exception ex)
                        {
                            DisqualificateGamer(currentGame, processes, 0, ex);
                            break;
                        }

                        try
                        {
                            stdout[1].WriteLine(ParsingFunctionsModule.PointsToString(previousPoints));
                            previousPoints = ParsingFunctionsModule.AnswerStringToPoint(await stdin[1].ReadLineWithTimeout(CommandTimeout));
                            PositionCheckerModule.CheckPosition(previousPoints, boardSize, boardArray, 1, currentGame);
                        }
                        catch (EmptyAnswerException)
                        {
                            if (PositionCheckerModule.IsAllBoardFull(boardArray, boardSize))
                            {
                                LoseGamer(currentGame, processes, 0);
                            }
                            else
                            {
                                DisqualificateGamer(currentGame, processes, 1, new EmptyAnswerException());
                            }
                            break;
                        }
                        catch (Exception ex)
                        {
                            DisqualificateGamer(currentGame, processes, 1, ex);
                            break;
                        }
                    }

                    GameHistorySaverModule.Save(currentGame);
                }

                Dispatcher.Invoke(() =>
                {
                    GameApplicationList = new ObservableCollection<GameApplication>(GameApplicationList.OrderByDescending(x => x.Result.WinNum).ThenBy(x => x.Result.DisNum).ToList());
                    IsBusy = false;
                });
            });
        }

        private void DisqualificateGamer(Game currentGame, Process[] processes, int num, Exception ex)
        {
            Dispatcher.Invoke(() =>
            {
                if (num == 0)
                {
                    currentGame.gamer1.Result.DisNum++;
                    currentGame.gamer2.Result.WinNum++;
                    ProcessManagmentModule.KillAllProcesses(processes);
                }
                else
                {
                    currentGame.gamer2.Result.DisNum++;
                    currentGame.gamer1.Result.WinNum++;
                    ProcessManagmentModule.KillAllProcesses(processes);
                }

                ErrorsList.Add(new Error(ex.Message, (num == 0 ? currentGame.gamer1.Name : currentGame.gamer2.Name)));
                GameHistorySaverModule.Save(currentGame);
            });
        }

        private void LoseGamer(Game currentGame, Process[] processes, int num)
        {
            Dispatcher.Invoke(() =>
            {
                if (num == 0)
                {
                    currentGame.gamer1.Result.LosNum++;
                    currentGame.gamer2.Result.WinNum++;
                    ProcessManagmentModule.KillAllProcesses(processes);
                }
                else
                {
                    currentGame.gamer2.Result.LosNum++;
                    currentGame.gamer1.Result.WinNum++;
                    ProcessManagmentModule.KillAllProcesses(processes);
                }

                GameHistorySaverModule.Save(currentGame);
            });
        }
    }
}
