﻿/*
Copyright (c) 2014, Kevin Pope
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies,
either expressed or implied, of the FreeBSD Project.
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using MatterHackers.Agg;
using MatterHackers.Agg.Platform;
using MatterHackers.Agg.UI;
using MatterHackers.Localizations;
using MatterHackers.MatterControl.Library;

namespace MatterHackers.MatterControl.CustomWidgets
{
	public class FolderBreadCrumbWidget : FlowLayoutWidget
	{
		private ListView listView;

		public FolderBreadCrumbWidget(ListView listView)
		{
			this.listView = listView;
			this.Name = "FolderBreadCrumbWidget";
			UiThread.RunOnIdle(() => SetBreadCrumbs(listView.ActiveContainer));

			this.HAnchor = HAnchor.Stretch;
			this.VAnchor = VAnchor.Fit | VAnchor.Center;
			this.Padding = new BorderDouble(left: 2);
		}

		public static IEnumerable<ILibraryContainer> ItemAndParents(ILibraryContainer item)
		{
			var container = item;
			while (container != null)
			{
				yield return container;
				container = container.Parent;
			}
		}

		public void SetBreadCrumbs(ILibraryContainer currentContainer)
		{
			var buttonFactory = ApplicationController.Instance.Theme.SmallMarginButtonFactory;

			var linkButtonFactory = ApplicationController.Instance.Theme.LinkButtonFactory;

			this.CloseAllChildren();

			bool haveFilterRunning = !string.IsNullOrEmpty(currentContainer.KeywordFilter);

			var theme = ApplicationController.Instance.Theme;

			var upbutton = theme.ButtonFactory.GenerateIconButton(AggContext.StaticData.LoadIcon(Path.Combine("FileDialog", "up_folder_20.png"), IconColor.Theme));
			upbutton.Name = "Library Up Button";
			upbutton.Margin = new BorderDouble(right: 2);
			upbutton.Click += (s, e) =>
			{
				if (listView.ActiveContainer.Parent != null)
				{
					UiThread.RunOnIdle(() => listView.SetActiveContainer(listView.ActiveContainer.Parent));
				}
			};

			this.AddChild(upbutton);

			bool firstItem = true;

			if (this.Width < 250)
			{
				Button containerButton = linkButtonFactory.Generate(listView.ActiveContainer.Name == null ? "?" : listView.ActiveContainer.Name);
				containerButton.Name = "Bread Crumb Button " + listView.ActiveContainer.Name;
				containerButton.VAnchor = VAnchor.Center;
				containerButton.Margin = new BorderDouble(right:  5);

				this.AddChild(containerButton);
			}
			else
			{
				foreach (var container in ItemAndParents(currentContainer).Reverse())
				{
					if (!firstItem)
					{
						// Add separator
						this.AddChild(new TextWidget(">", textColor: ActiveTheme.Instance.PrimaryTextColor)
						{
							VAnchor = VAnchor.Center,
							Margin = new BorderDouble(right: 5)
						});
					}

					Button gotoProviderButton =  linkButtonFactory.Generate(container.Name);
					gotoProviderButton.Name = "Bread Crumb Button " + container.Name;
					gotoProviderButton.VAnchor = VAnchor.Center;
					gotoProviderButton.Margin = new BorderDouble(right:  5);
					gotoProviderButton.Click += (s, e) =>
					{
						UiThread.RunOnIdle(() => listView.SetActiveContainer(container));
					};
					this.AddChild(gotoProviderButton);

					firstItem = false;
				}

				if (haveFilterRunning)
				{
					// Add separator ;
					this.AddChild(new TextWidget(">", textColor: ActiveTheme.Instance.PrimaryTextColor)
					{
						VAnchor = VAnchor.Center,
						Margin = new BorderDouble(right: 5)
					});

					Button searchResultsButton = null;
					if (UserSettings.Instance.IsTouchScreen)
					{
						searchResultsButton = buttonFactory.Generate("Search Results".Localize(), AggContext.StaticData.LoadIcon("icon_search_32x32.png", IconColor.Theme));
					}
					else
					{
						searchResultsButton = buttonFactory.Generate("Search Results".Localize(), AggContext.StaticData.LoadIcon("icon_search_24x24.png", IconColor.Theme));
					}
					searchResultsButton.Name = "Bread Crumb Button " + "Search Results";
					searchResultsButton.Margin = new BorderDouble(right:  5);
					this.AddChild(searchResultsButton);
				}

				// while all the buttons don't fit in the control
				if (this.Parent != null
					&& this.Width > 0
					&& this.Children.Count > 4
					&& this.GetChildrenBoundsIncludingMargins().Width > (this.Width - 20))
				{
					// lets take out the > and put in a ...
					this.RemoveChild(1);

					var separator = new TextWidget("...", textColor: ActiveTheme.Instance.PrimaryTextColor)
					{
						VAnchor = VAnchor.Center,
						Margin = new BorderDouble(right:  5)
					};
					this.AddChild(separator, 1);

					while (this.GetChildrenBoundsIncludingMargins().Width > this.Width - 20
						&& this.Children.Count > 4)
					{
						this.RemoveChild(3);
						this.RemoveChild(2);
					}
				}
			}
		}
	}
}