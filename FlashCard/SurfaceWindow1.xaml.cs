/*
"Flash Card" 
An generic flash card application for the Microsoft Surface that is easily customized.

The MIT License

Copyright (c) 2009 William C. Kurt

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media.Animation;
using System.Xml;

namespace FlashCard
{
    //

    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        DirectoryInfoReader externalModelDirectory;
        ExternalModelData modelData;
        int currentTag;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            //change this to change default directory for the card data
            string cardDataDirectory = @"C:\flash_card_data";
            externalModelDirectory = new DirectoryInfoReader(cardDataDirectory);
            modelData = new ExternalModelData(externalModelDirectory.ExternalModels);
            currentTag = -1;
            InitializeComponent();
            //scatterView1.Items.Add(svi);

            // Add handlers for Application activation events
            AddActivationHandlers();
        }


        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for Application activation events
            RemoveActivationHandlers();
        }

        /// <summary>
        /// Adds handlers for Application activation events.
        /// </summary>
        private void AddActivationHandlers()
        {
            // Subscribe to surface application activation events
            ApplicationLauncher.ApplicationActivated += OnApplicationActivated;
            ApplicationLauncher.ApplicationPreviewed += OnApplicationPreviewed;
            ApplicationLauncher.ApplicationDeactivated += OnApplicationDeactivated;
        }

        /// <summary>
        /// Removes handlers for Application activation events.
        /// </summary>
        private void RemoveActivationHandlers()
        {
            // Unsubscribe from surface application activation events
            ApplicationLauncher.ApplicationActivated -= OnApplicationActivated;
            ApplicationLauncher.ApplicationPreviewed -= OnApplicationPreviewed;
            ApplicationLauncher.ApplicationDeactivated -= OnApplicationDeactivated;
        }

        /// <summary>
        /// This is called when application has been activated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationActivated(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when application is in preview mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationPreviewed(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        ///  This is called when application has been deactivated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationDeactivated(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        //Logic for adding items to the scatterview
        private void checkAndAddModel(object sender, ContactEventArgs e)
        {
            //make sure that the item is not already being displayed
            if (e.Contact.IsTagRecognized && e.Contact.Tag.Type == TagType.Byte && (int)e.Contact.Tag.Byte.Value != currentTag)
            {

                ExternalModel myModel = modelData.GetOrganFromTag(e.Contact.Tag.Byte.Value);
                currentTag = (int)e.Contact.Tag.Byte.Value;
                ClearInfo();
                if (myModel != null)
                {
                    //AddMediaElementsToScatterView(myModel.GetImagesNodes());
                    AddSVIsToScatterView(myModel.GetSVIs());
                    //organNameLabel.Content = myModel.title;
                    //organDescriptionBox.Text = myModel.description;
                }
            }
        }

        private void AddSVIsToScatterView(ScatterViewItem[] svis)
        {
            foreach (ScatterViewItem item in svis)
            {
                SurfaceButton button = (SurfaceButton)item.FindName(item.Name.Replace("_SVI", "_FLIPBUTTON"));
                button.Click += new RoutedEventHandler(FlipImage);
                scatterView1.Items.Add(item);

            }

        }
  

        //clears the info about a previous object
        private void ClearInfo()
        {
            scatterView1.Items.Clear();
        }



        //trying out a flip button
        private void FlipImage(object sender, RoutedEventArgs e)
        {
            //later one we can add the name root to the button,
            //it can then look up the viewboxes based on that.
            Viewbox labeledBox = (Viewbox)((Grid)((SurfaceButton)sender).Parent).FindName(((SurfaceButton)sender).Name.Replace("_FLIPBUTTON", "_L"));
            Viewbox unlabeledBox = (Viewbox)((Grid)((SurfaceButton)sender).Parent).FindName(((SurfaceButton)sender).Name.Replace("_FLIPBUTTON", "_U"));
            if (labeledBox.IsVisible)
            {
                labeledBox.Visibility = Visibility.Hidden;
                unlabeledBox.Visibility = Visibility.Visible;
            }
            else
            {
                labeledBox.Visibility = Visibility.Visible;
                unlabeledBox.Visibility = Visibility.Hidden;
            }


        }


    }
}