﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TranslateApplication
{
    public static class Configs
    {
        public const string DIRECTORY_NAME = "TranslationFiles";

        public const string BASE_DICTIONARY_FILENAME = "BaseDictionary.txt";
        public const string LEARNED_WORDS_DICTIONARY_FILENAME = "LearnedWordsDictionary.txt";
        public const string NOT_LEARNED_WORDS_DICTIONARY_FILENAME = "NotLearnedWordsDictionary.txt";

        public static string CurrentDrive { get; private set; }
        public static string PathToDirectory { get; private set; }

        public static string PathToBaseDictionaryFile { get; private set; }
        public static string PathToLearnedWordsFile { get; private set; }
        public static string PathToNotLearnedWordsFile { get; private set; }

        public static string[] GetAllWordsFrom(TranslatorFiles fileType)
        {
            switch (fileType)
            {
                case TranslatorFiles.BaseDirectory:return File.ReadAllLines(PathToBaseDictionaryFile);
                case TranslatorFiles.LearnedWords:return File.ReadAllLines(PathToLearnedWordsFile);
                default :return File.ReadAllLines(PathToNotLearnedWordsFile);
            }
        }

        public static void ReSaveFile(TranslatorFiles fileType, IEnumerable<string> words)
        {
            switch (fileType)
            {
                case TranslatorFiles.BaseDirectory:
                    using (var stream = File.CreateText(PathToBaseDictionaryFile))
                    {
                        foreach (var word in words)
                        {
                            stream.WriteLine(word);
                        }
                    }
                break;
                case TranslatorFiles.LearnedWords:
                    using (var stream = File.CreateText(PathToLearnedWordsFile))
                    {
                        foreach (var word in words)
                        {
                            stream.WriteLine(word);
                        }
                    }
                    break;
                default:
                    using (var stream = File.CreateText(PathToNotLearnedWordsFile))
                    {
                        foreach (var word in words)
                        {
                            stream.WriteLine(word);
                        }
                    }
                    break;
            }
        }
        public static void AddToFile(TranslatorFiles fileType, params string[] words)
        {
                switch (fileType)
                {
                    case TranslatorFiles.BaseDirectory:
                        File.AppendAllLines(PathToBaseDictionaryFile, words);
                        break;
                    case TranslatorFiles.LearnedWords:
                        File.AppendAllLines(PathToLearnedWordsFile, words);
                        break;
                    default:
                        File.AppendAllLines(PathToNotLearnedWordsFile, words);
                        break;
                }
        }

        private static bool HasWriteAccessToFolder(string folderPath)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
        public static void Init()
        {
            foreach(var drive in DriveInfo.GetDrives())
            {
                if (HasWriteAccessToFolder(drive.Name))
                {
                    CurrentDrive = drive.Name;
                    break;
                }
            }
            PathToDirectory = CurrentDrive + @"\" + DIRECTORY_NAME;

            if (!Directory.Exists(PathToDirectory))
            {
                Directory.CreateDirectory(PathToDirectory);
            }
            PathToBaseDictionaryFile = PathToDirectory + @"\" + BASE_DICTIONARY_FILENAME;
            if (!File.Exists(PathToBaseDictionaryFile))
            {
                File.Copy(BASE_DICTIONARY_FILENAME,PathToBaseDictionaryFile);
            }
            PathToLearnedWordsFile = PathToDirectory + @"\" + LEARNED_WORDS_DICTIONARY_FILENAME;
            if (!File.Exists(PathToLearnedWordsFile))
            {
                File.Create(PathToLearnedWordsFile);
            }
            PathToNotLearnedWordsFile = PathToDirectory + @"\" + NOT_LEARNED_WORDS_DICTIONARY_FILENAME;
            if (!File.Exists(PathToNotLearnedWordsFile))
            {
                File.Copy(BASE_DICTIONARY_FILENAME, PathToNotLearnedWordsFile);
            }
        } //Всегда один раз запускайте этот метод!
    }
}
