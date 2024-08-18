#if !USE_ORIGINAL_CODE
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Terraria
{
    internal class Settings
    {
        private List<string> Subsections = new List<string>();
        private List<string> Keys = new List<string>();
        private List<string> Values = new List<string>();
        private List<string> Comments = new List<string>();

        readonly FileInfo Path;
        readonly string Executable = Assembly.GetExecutingAssembly().GetName().Name;

        public Settings(string File)
        {
            using (StreamWriter Writer = new StreamWriter(File, true)) // The 'using' will close the writer after Path is declared, so that the reader in Load() won't clash with the settings file due to FileInfo.
            {
                Path = new FileInfo(File ?? Executable);
            }
            Load();
        }

        public void Set(string Subsection, string Key, string Value)
        {
            for (int i = 0; i < Keys.Count; i++)
            {
                if (Keys[i].Equals(Key))
                {
                    Subsections[i] = Subsection;
                    Values[i] = Value;
                    return;
                }
            }

            Subsections.Add(Subsection);
            Keys.Add(Key);
            Values.Add(Value);
            Comments.Add(""); // Comments are accounted for regardless due to the setup but this makes sure we have no issues.
        }

        public void Set(string Subsection, string Key, string Value, string Comment)
        {
            for (int i = 0; i < Keys.Count; i++)
            {
                if (Keys[i].Equals(Key))
                {
                    Subsections[i] = Subsection;
                    Values[i] = Value;
                    Comments[i] = Comment;
                    return;
                }
            }

            Subsections.Add(Subsection);
            Keys.Add(Key);
            Values.Add(Value);
            Comments.Add(Comment);
        }

        public string Get(string Subsection, string Key)
        {
            for (int i = 0; i < Keys.Count; i++)
            {
                if (Subsections[i].Equals(Subsection) && Keys[i].Equals(Key))
                {
                    return Values[i];
                }
            }
            return "";
        }

        public bool AlreadyExists(string Subsection, string Key)
        {
            return Get(Subsection, Key).Length > 0;
        }

        /*public void Remove(string Subsection, string Key)     // No use here but here it is if I need it
        {
            for (int i = 0; i < Keys.Count; i++)
            {
                if (Keys[i].Equals(Key) && Subsections[i].Equals(Subsection)) // Remove 'Subsections[i].Equals(subSection)' to remove just from a given key
                {
                    Subsections.RemoveAt(i);
                    Keys.RemoveAt(i);
                    Values.RemoveAt(i);
                    Comments.RemoveAt(i);
                    return;
                }
            }
        }*/

        public void Load()
        {
            Subsections = new List<string>();
            Keys = new List<string>();
            Values = new List<string>();
            Comments = new List<string>();

            string Line = "", Category = "";
            using (StreamReader Reader = new StreamReader(Path.FullName))
            {
                string PreviousLine = "";
				string CurrentLine = "";
                string Comment = "";
				while ((Line = Reader.ReadLine()) != null)
                {
                    int ValueOffset = Line.IndexOf("=");
                    int CommentOffset = Line.IndexOf(";");
                    if (Line.IndexOf("[") == 0)
                    {
                        Category = Line.Substring(1, Line.Length - 2);
                    }

                    if (CommentOffset != -1 && ValueOffset == -1)
                    {
						PreviousLine = CurrentLine;
                        CurrentLine = Line.Substring(CommentOffset + 1).TrimStart(' ');

                        if (PreviousLine != "")
                        {
                            PreviousLine += "\n; ";
                        }

                        Comment = PreviousLine + CurrentLine;
                    }

                    if (ValueOffset > 0)
                    {
                        if (Comment != "")
                        {
							Set(Category, Line.Substring(0, ValueOffset), Line.Substring(ValueOffset + 1), Comment);
						}
                        else
                        {
							Set(Category, Line.Substring(0, ValueOffset), Line.Substring(ValueOffset + 1));
						}

						PreviousLine = "";
						CurrentLine = "";
						Comment = "";
					}
                }
            }
        }

        public void Close()
        {
            using (StreamWriter Writer = new StreamWriter(Path.FullName))
            {
                List<string> UniqueSubsections = new List<string>();
                for (int i = 0; i < Subsections.Count; i++)
                {
                    if (!UniqueSubsections.Contains(Subsections[i]))
                    {
                        UniqueSubsections.Add(Subsections[i]);
                    }
                }

                List<string> SubsectionsToWrite = Subsections;
                List<string> KeysToWrite = Keys;
                List<string> ValuesToWrite = Values;
                List<string> CommentsToWrite = Comments;
                string Entry;
                for (int i = 0; i < UniqueSubsections.Count; i++)
                {
                    bool FirstKeyInSect = true;
                    while (SubsectionsToWrite.Contains(UniqueSubsections[i]))
                    {
                        int Index = SubsectionsToWrite.IndexOf(UniqueSubsections[i]);
                        if (FirstKeyInSect)
                        {
                            if (!UniqueSubsections[i].Equals(""))
                            {
                                if (i > 0)
                                {
                                    Writer.WriteLine(); // Neatness
                                }

                                Entry = string.Format("[{0}]", UniqueSubsections[i]);
                                Writer.WriteLine(Entry);
                                FirstKeyInSect = false; // Only include the subsection listing if its the first entry in the subsection.
                            }
                        }
                        if (!CommentsToWrite[Index].Equals(""))
                        {
                            Entry = string.Format("; {0}\n{1}={2}\n", CommentsToWrite[Index], KeysToWrite[Index], ValuesToWrite[Index]);
                            Writer.WriteLine(Entry);
                        }
                        else
                        {
                            Entry = string.Format("{0}={1}\n", KeysToWrite[Index], ValuesToWrite[Index]);
                            Writer.WriteLine(Entry);
                        }
                        SubsectionsToWrite.RemoveAt(Index);
                        KeysToWrite.RemoveAt(Index);
                        ValuesToWrite.RemoveAt(Index);
                        CommentsToWrite.RemoveAt(Index);
                    }
                }
            }
        }
    }
}
#endif