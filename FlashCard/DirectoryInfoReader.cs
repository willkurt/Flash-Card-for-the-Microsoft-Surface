using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.IO;


namespace FlashCard
{
    /*
     * The idea behind this class is that all the information about
     * particular models can actually be stored with the directory structures
     * 
     * There is a root directory, and with in that is a bunch of 
     * subdirectories for each model
     * each model folder is of the form NUMBER-NAME
     * in each folder is an 'images' subfolder
     * 
     * Earlier versions had a 'notes' folder, this has been left as an option
     * in case anyone wants to add this feature later on.
     */ 
    class DirectoryInfoReader
    {
        string directorypath;
        string[] modelFolders;
        List<ExternalModel> externalModels = new List<ExternalModel> { };

        public DirectoryInfoReader(string directorypath)
        {
            this.directorypath = directorypath;
            this.modelFolders = Directory.GetDirectories(this.directorypath);
            /*
             * Each folder represents one model
             * 
             */
            foreach (string folder in this.modelFolders)
            {
                string folderName = folder.Split('\\').Last();
                string[] numberTitle = folderName.Split('-');
                int number = int.Parse(numberTitle[0]);
                string title = numberTitle[1];
                string[] images = Directory.GetFiles(folder+"\\images");
                string[] notes = new string[]{};
                if (Directory.Exists(folder+"\\notes"))
                {
                    notes = Directory.GetFiles(folder + "\\notes");
                }
                ExternalModel next = new ExternalModel(number, title, images, notes);
                this.externalModels.Add(next);
              }
        }   
            
        public List<ExternalModel> ExternalModels
            {
                get
                {
                    return this.externalModels;
                }

                
              
            
            }

            
        }   

    }

