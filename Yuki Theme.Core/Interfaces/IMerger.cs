using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Interfaces;

public interface IMerger
{
	void MergeThemeFieldsWithFile (Dictionary<string, ThemeField> local, XmlDocument doc);
	void SaveXML (Image img2, Image img3, bool wantToKeep, bool iszip, ref XmlDocument doc, string themePath);
	void MergeCommentsWithFile (Theme themeToMerge, XmlDocument doc);
}