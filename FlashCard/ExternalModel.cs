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
using System.Windows.Markup;
using System.Xml;
using System.Text.RegularExpressions;

namespace FlashCard
{
    class ExternalModel
    {
        public int  tag;
        public string title;
        public string[] notePaths;
        public string[] imageUris;
        public List<string> imageUrisLabeled;
        public List<string> imageUrisUnlabeled;

        public ExternalModel(int tag, string title, string[] imageUris, string[] notePaths)
        {
            this.tag = tag;
            this.title = title;
            this.notePaths = notePaths;
            this.imageUris = imageUris;
            this.imageUrisLabeled = new List<string>{ };
            this.imageUrisUnlabeled = new List<string> { };
            foreach (string address in imageUris)
            {
               if(LabeledCondition(address))
               {
                imageUrisLabeled.Add(address);
               }
               else if (UnlabeledCondition(address))
               {
                imageUrisUnlabeled.Add(address);
               }
                //not 100% sure what I want to be the final else conditions
                //this would be a good place to catch things that don't
                //clearly fall in the labeled/unlabeled category
            }

        }

        /*
         * Overall I'm not too happy with the level of abstraction here
         * the heuristic to determine which image is labeled and which is not
         * doesn't seem quite obvious, so I've done my best put this logic
         * in as modular of an area as I can in order to be able to understand 
         * this in the future
         * 
         * 
         * the current model is
         * filename-U.jpg -> Unlabeled version (so we're looking for '-U.' in the filename
         * filename-L.jpg -> Labeled version (so we're looking for '-L.' in the filename
         * 
         */ 


        //this method check against the property which determines whether or not an
        //image is labeld or not
        private static bool LabeledCondition(string uri)
        {
            string filename = uri.Split('\\').Last();
                //we may need to tweak this but probably not
            if (filename.Contains("-L."))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool UnlabeledCondition(string uri)
        {
            string filename = uri.Split('\\').Last();
            //we may need to tweak this but probably not
            if (filename.Contains("-U."))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

        //I also want to flip logic in the class, so that if the logic
        //of determining labeled v. unlabled changes, we can just change it here
        public static string FlipImageUri(string originalUri)
        {
            string flippedUri;
            if (LabeledCondition(originalUri))
            {
                flippedUri = originalUri.Replace("-L.", "-U.");
            }
            else if (UnlabeledCondition(originalUri))
            {
                flippedUri = originalUri.Replace("-U.", "-L.");
            }
            else
            {
                flippedUri = originalUri;
            }

            return flippedUri;
        }

        public MediaElement[] GetImagesNodes()
        {
            string[] labeledUris = this.imageUrisLabeled.ToArray();
            MediaElement[] images = new MediaElement[labeledUris.Length];
            for (int i = 0; i < images.Length; i++)
            {
                MediaElement imageNode = new MediaElement();
                imageNode.Source = new Uri(labeledUris[i]);
                images[i] = imageNode;
            }
            
            return images;
        }

        
        //this should hopefully build the scatter view items
        //minus their appropriate even handlers
        public ScatterViewItem[] GetSVIs()
        {
            string[] labeledUris = this.imageUrisLabeled.ToArray();
            XmlReader[] generatedXaml = new XmlReader[labeledUris.Length];
            ScatterViewItem[] generatedSVIs = new ScatterViewItem[labeledUris.Length];
            for (int i = 0; i < generatedXaml.Length; i++)
            {
                //relying heavily on naming conventions here
                //but I think this will be the most helpful solutions
                //every piece will have a -X. part that will explain what
                //it does


                string labeledSource = labeledUris[i];
                string unlabeledSource = labeledSource.Replace("-L.","-U.");

                //needs to start with a letter so we'll add 'i'
                string imageRoot = "i"+Regex.Split((Regex.Split(labeledSource, "images").Last()), "-L")[0].Replace("\\","").Replace("-","");

                string labeledName = imageRoot + "_L";
                string unlabeledName = imageRoot + "_U";
                string sviName = imageRoot + "_SVI";
                string flipButtonName = imageRoot+"_FLIPBUTTON";

                
                string xamlDoc = @" <s:ScatterViewItem 
            xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
            xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
            xmlns:s='http://schemas.microsoft.com/surface/2008'
                 MinWidth='100' MaxWidth='1200' Name='"+sviName+@"'>
                <Grid>
                    <!-- Row and column definitions to position content and buttons -->
                    <Grid.RowDefinitions>
                        <RowDefinition Height='30'/>
                        <RowDefinition />
                        <RowDefinition Height='30'/>
                    </Grid.RowDefinitions>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width='30'/>
                        <ColumnDefinition/>
                        <ColumnDefinition Width='30'/>
                        <ColumnDefinition Width='30'/>
                        <ColumnDefinition Width='30'/>
                        <ColumnDefinition/>
                        <ColumnDefinition Width='30'/>
                    </Grid.ColumnDefinitions>"+@"

                    <!-- The Ink Canvas -->
                    <Grid Grid.Column='0' Grid.ColumnSpan='7' Grid.Row='0' Grid.RowSpan='5'>

                        <Viewbox Name='"+labeledName+@"' StretchDirection='Both' Stretch='Uniform' Visibility='Hidden' >
                            <Image Source='"+labeledSource+@"' />
                        </Viewbox>
                        <Viewbox Name='"+unlabeledName+@"' Visibility='Visible' StretchDirection='Both' Stretch='Uniform' >
                            <Image Source='"+unlabeledSource+@"' />
                        </Viewbox>    
                    </Grid>

                    <!-- Clear -->
                    <s:SurfaceButton Grid.Row='2' Grid.Column='2' Padding='5'  Name='"+flipButtonName+ @"'>
                        <Image Source='Resources\Flip.png' />
                    </s:SurfaceButton>

                    <!-- I don't need this button now, but I might want it later
                    <s:SurfaceButton Name='SpaceHolder' Grid.Row='2' Grid.Column='4' Padding='5'>
                        <Image Source='Resources\Move.png' />
                    </s:SurfaceButton>-->
                </Grid>
            </s:ScatterViewItem>";

                StringReader sr = new StringReader(xamlDoc);
                XmlReader xr = XmlReader.Create(sr);
                ScatterViewItem svi = (ScatterViewItem)XamlReader.Load(xr);
                generatedSVIs[i] = svi;
            }
            return generatedSVIs;
        }
    }
}
