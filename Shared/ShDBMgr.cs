using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using SharedCode.Resources;
using View = Autodesk.Revit.DB.View;

using static UtilityLibrary.MessageUtilities2;



// v 3.0.2.0	Adjust to make dependent views when applies

// shared database manager

namespace SharedCode
{
	public class ShDbMgr
	{
		public ViewSheet LastNewSheet;

		private const char TBlkSeperator = (char) 0x00A0;


		internal enum FmtViewName { NONE = 0, GROUP = 1, DIVIDE = 2 }

		#region + Private

		private static Document   _doc;
		private UIDocument _uidoc;
		private static Categories _cats;

		// save the list of ViewSheets
		private static FilteredElementCollector _vs;

		// a list of all sheet numbers - lower case
		private string[] _sheetNumberList;

		private string[] _viewNameList;

		private static FilteredElementCollector _mVw;

		private static double _parentLeft;
		private static double _parentTop;

		#endregion

		#region + Constructor

		public ShDbMgr(ExternalCommandData commandData)
		{
			UIApplication uiapp = commandData.Application;
			_uidoc = commandData.Application.ActiveUIDocument;
			_doc = _uidoc.Document;

			GetAllSheetNumbers();

			_cats = _doc.Settings.Categories;
		}

		#endregion

		#region + Procedures

		public bool Process2(NewSheetFormat nsf)
		{
			bool result = false;
			int seq = 0;

			LastNewSheet = null;

			try
			{
				nsf.TitleBlockName = SelectTitleBlock(nsf);

				for (int i = 1; i < nsf.Copies + 1; i++)
				{
					// part A - determine the new sheet number depending
					// on newsheetformat
					// find sheet number and determine the sequence number
					nsf.newSheetNumber = FindNextSheetNumber(nsf, seq, out nsf.seq);

					// part B - determine the new sheet name depending
					// on newsheetformat
					nsf.newSheetName = GetNewSheetName(nsf);

					// got the new sheet number and name

					switch (nsf.OperationOption)
					{
					case OperOpType.CreateEmptySheets:
						{
							CreateOneEmptySheet(nsf);
							result = true;
							break;
						}
					case OperOpType.DupSheet:
						{
							DuplicateOneSheet(nsf);
							result = true;
							break;
						}
					case OperOpType.DupSheetAndViews:
						{
							DuplicateOneSheet(nsf);
							result = true;
							break;
						}
					}

					// make sure that the list of sheet numbers is current
					// and includes the sheet just created
					GetAllSheetNumbers();
				}
			}
			catch (Exception e)
			{
				ShUtil.ShowExceptionDialog(e, nsf, _parentLeft, _parentTop);

				

				return false;
			}

			return result;
		}


		#region + Create One Sheet

		// create a single blank sheet with a title block - if
		// wanted and using the parameters from the 
		// selected sheet if wanted
		public ViewSheet CreateOneEmptySheet(NewSheetFormat nsf)
		{
			ViewSheet vsDestinationSheet;

			ElementId tbId = ElementId.InvalidElementId;

			if (nsf.TitleBlockName != null)
			{
				tbId = GetTitleBlockFs(nsf.TitleBlockName).Id;
			}

			// create a sheet
			vsDestinationSheet = ViewSheet.Create(_doc, tbId);

			if (vsDestinationSheet == null)
			{
				throw new Exception(AppStrings.R_ErrCreateSheetFailDesc);
			}

			// give the new sheet a number and name
			vsDestinationSheet.SheetNumber = nsf.newSheetNumber;
			vsDestinationSheet.Name = nsf.newSheetName;


			if (nsf.UseParameters && nsf.SelectedSheet.SheetView != null)
			{
				CopyParameters(nsf.FcSelViewSht, vsDestinationSheet);
			}

			return vsDestinationSheet;
		}

		#endregion

		#region + Duplicate One Sheet

		/// <summary>
		/// Duplicate one existing sheet
		/// </summary>
		/// <param name="shtNumber">The new sheet number</param>
		/// <param name="shtName">The new sheet name</param>
		/// <param name="tbName">The title block for the new sheet</param>
		/// <param name="nsf.FcSelViewSht">The ViewSheet to copy</param>
//		public void DuplicateOneSheet(string shtNumber, string shtName, string tbName, ViewSheet vsSourceSheet)
		public void DuplicateOneSheet(NewSheetFormat nsf)
		{

			ViewSheet vsDestinationSheet;

			try
			{
				// try to create the sheet
				// see below about copy parameters
				vsDestinationSheet = CreateOneEmptySheet(nsf);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			// at this point we have a source and destination ViewSheet
			// here we copy the elements on the source ViewSheet and
			// place them onto the destination ViewSheet
			// except, we don't copy the title block as this was added
			// when the ViewSheet was created

			// first, get a collection of elements that exist on the source sheet
			FilteredElementCollector colViewSheet = 
				new FilteredElementCollector(_doc, nsf.FcSelViewSht.Id);

			// prepare for the elements to "copy" and "paste"
			ICollection<ElementId> copyIds = new List<ElementId>();

			// this may fail, prepare for the failure and capture the failure message
			try
			{
				// in order to copy detail lines directly onto the sheet, we need to 
				// setup a SketchPlane on the "blank" sheet

				// SketchPlane sketch = CreateSketchPlane(vsDestinationSheet);

				List<Viewport> vpList       = new List<Viewport>(10);
				List<Viewport> vpListNoCopy = new List<Viewport>(10);

				copyIds.Clear();

				// the ViewSheet has been setup to copy / paste or duplicate the elements
				// held by the ViewSheet - with the exceptions that ViewPorts cannot be copied
				foreach (Element sourceElem in colViewSheet)
				{
					// ignore GuideGrids
					if (sourceElem.Category.Id == _cats.get_Item(BuiltInCategory.OST_GuideGrid).Id)
						continue;

					// is the current element a titleblock? - this is the source titleblock
					if (sourceElem.Category.Id == _cats.get_Item(BuiltInCategory.OST_TitleBlocks).Id)
					{
						ConfigureNewTitleBlock(sourceElem, vsDestinationSheet);
						continue;
					}

					// we cannot copy some viewports or GuideGrids
					if (sourceElem.Category.Id == _cats.get_Item(BuiltInCategory.OST_Viewports).Id)
					{
						Viewport vp = (Viewport) sourceElem;
						View     vw = (View) _doc.GetElement(vp.ViewId);

						if (Viewport.CanAddViewToSheet(_doc, vsDestinationSheet.Id, vw.Id))
						{
							vpList.Add(vp); // got a viewport to copy
						}
						else
						{
							vpListNoCopy.Add(vp); // got a viewport I cannot copy
						}
						continue;
					}

					// process a not viewport items
					copyIds.Add(sourceElem.Id);
				}

				// preform the actual copy of the elements
				if (copyIds.Count > 0)
				{
					ElementTransformUtils.CopyElements((ViewSheet) nsf.SelectedSheet.SheetView, copyIds, vsDestinationSheet,
						Transform.Identity, new CopyPasteOptions());
				}

				// processed all of the items on the view sheet except for the viewports
				// we have a list of viewports we can copy and place onto the sheet
				// here we process the list of viewports that cannot be copy / pasted and replicate
				// them matching their original location
				if (vpList.Count > 0)
				{
					foreach (Viewport vpSource in vpList)
					{
						View vw = _doc.GetElement(vpSource.ViewId) as View;

						PlaceViewportOnSheet(vsDestinationSheet, vpSource, vw);

					}
				}

				if (vpListNoCopy.Count > 0 && nsf.OperationOption == OperOpType.DupSheetAndViews)
				{
					// get all of the view names here to make sure it is current
					GetAllViewNames();

					foreach (Viewport vpSource in vpListNoCopy)
					{
						View vwSource = _doc.GetElement(vpSource.ViewId) as View;

						if (!vwSource.CanViewBeDuplicated(ViewDuplicateOption.WithDetailing))
						{
							continue;
						}

						View vwCopy;

						// if PrimaryViewId is not InvalidElementId, view is dependant
						if (vwSource.GetPrimaryViewId() == ElementId.InvalidElementId)
						{
							vwCopy = _doc.GetElement(vwSource.Duplicate(ViewDuplicateOption.WithDetailing)) as View;
						}
						else
						{
							vwCopy = _doc.GetElement(vwSource.Duplicate(ViewDuplicateOption.AsDependent)) as View;
						}

						if (vwCopy != null)
						{
							vwCopy.Name = FindNextViewName(vwSource.Name, nsf);
//							vwCopy.Name = FindNextViewName(vwSource.Name, nsf.newSheetNumber);

							PlaceViewportOnSheet(vsDestinationSheet, vpSource, vwCopy);
						}
					}
				}

				LastNewSheet = vsDestinationSheet;

			}
			catch (Exception e)
			{
				throw new Exception(AppStrings.R_ErrDupSheetFailDesc + nl + e.Message);
			}

		}

		#endregion

		#endregion

		#region + Properties

		public double ParentLeft
		{
			get => _parentLeft;
			set
			{
				if (value >= 0)
					_parentLeft = value;
			}
		}

		public double ParentTop
		{
			get => _parentTop;
			set
			{
				if (value >= 0)
					_parentTop = value;
			}
		}

		internal string SheetNamePrefix    { private get; set; } = "";
		internal string SheetNumPrefix     { private get; set; } = "";
		internal string SheetNumFormat     { private get; set; } = "";
		internal bool   SheetNameIncrement { private get; set; }

		internal bool        SetFmtAddSpace       { private get; set; } = true;
		internal string      SetDivideString      { private get; set; } = "-";
		internal string      SetGroupStringLeft   { private get; set; } = "(";
		internal string      SetGroupStringRight  { private get; set; } = ")";
		internal FmtViewName SetGroupDivideOrNone { private get; set; } = FmtViewName.NONE;

		public View ActiveView => _doc.ActiveView;


		public View ActiveGraphicalView
		{
			get
			{
				IList<UIView> uiviews = _uidoc.GetOpenUIViews();

				return (View)_doc.GetElement(uiviews[0].ViewId);
			}
		}

		public int SheetCount => _vs.GetElementCount();

		public int TitleBlockCount => GetAllTitleBlocks().Count();

		#endregion


		#region + Public-Internal Methods

		public FilteredElementCollector AllViewSheets()
		{
			return _vs;
			//.OfCategory(BuiltInCategory.OST_Sheets
			//.OfClass(typeof(s);
		}

		public ViewSheet FindExistSheet(string shtNumber)
		{
			
			if (shtNumber == null) return null;

			foreach (ViewSheet vs in _vs)
			{
				if (vs.SheetNumber.Equals(shtNumber))
				{
					return vs;
				}
			}
			return null;
		}

		internal string FormatViewName(string origViewName, string shtNumber, int idx)
		{
			string viewName = origViewName;

			if (SetGroupDivideOrNone == FmtViewName.NONE)
			{
				if (SetFmtAddSpace)
				{
					viewName += " ";
				}
				viewName += shtNumber;
			}
			else if (SetGroupDivideOrNone == FmtViewName.DIVIDE)
			{
				if (SetFmtAddSpace)
				{
					viewName += " ";
				}
				viewName += SetDivideString;
				if (SetFmtAddSpace)
				{
					viewName += " ";
				}

				viewName += shtNumber;
			}
			else if (SetGroupDivideOrNone == FmtViewName.GROUP)
			{
				if (SetFmtAddSpace)
				{
					viewName += " ";
				}
				viewName += SetGroupStringLeft
					+ shtNumber
					+ SetGroupStringRight;
			}

			if (idx > 0)
			{
				viewName += " " + idx;
			}

			return viewName;
		}

		/// <summary>
		/// format a sheet number
		/// </summary>
		/// <param name="idx"></param>
		/// <returns></returns>
		internal string FormatSheetNumber(int idx)
		{
			return SheetNumPrefix + idx.ToString(SheetNumFormat);
		}

		public static string FormatTitleBlockName(FamilySymbol fs)
		{
			

			return fs.Family.Name + TBlkSeperator + '♦' + TBlkSeperator + fs.Name;
		}

		public FilteredElementCollector GetAllTitleBlocks()
		{
			return new FilteredElementCollector(_doc).OfClass(typeof (FamilySymbol)).OfCategory(BuiltInCategory.OST_TitleBlocks);
		}

		#endregion

		#region + Utilities

		public bool IsViewDrawingSheet(View v)
		{
			return v.ViewType.Equals(ViewType.DrawingSheet);
		}

		// figure out the next valid sheet number
		// validate against the current set of
		// sheet numbers - new sheet number
		// cannot be one of these
		private string FindNextSheetNumber(  NewSheetFormat nsf, int seq, out int seq1)
		{
			int idx = 0;
			seq1 = seq;

			string newSheetNumber = null;

			bool result;

			do
			{
				switch (nsf.NewSheetOption)
				{
				case NewShtOptions.PerSettings:
					{
						newSheetNumber = ShNewSheetMgr.
							FormatShtNumber(nsf.SheetFormatPs.NumberPrefix,
								nsf.PsNumFmtCode, ++seq1);
						break;
					}
				default: // from current
					{
						newSheetNumber = ShNewSheetMgr.
							FormatShtNumber(
								nsf.SelectedSheet.SheetNumber,
								nsf.FcNumDivChar,
								nsf.FcNumDivCharCustom,
								nsf.FcNumSuffix,
								++seq1);
						break;
					}
				}

				idx = Array.IndexOf(_sheetNumberList, newSheetNumber.ToLower(), idx);

				result = (idx++ >= 0);

			} while (result);
			
			return newSheetNumber;
		}

		// determine the next sheet number based on a starting number
		private int FindNextSheetNumber(int shtNumIdx)
		{
			string shtNumber;

			int idx = 0;

			bool result;

			do
			{
				shtNumber = FormatSheetNumber(++shtNumIdx).ToLower();

				idx = Array.IndexOf(_sheetNumberList, shtNumber, idx);

				result = (idx++ >= 0);

			} while (result);

			return shtNumIdx;
		}

		private string FindNextViewName(string origViewName, NewSheetFormat nsf)
		{
			string baseName = FormatViewName(origViewName, nsf);
			string proposedViewName = baseName;

			int idx = 1;

			while (Array.IndexOf(_viewNameList, proposedViewName) >= 0)
			{
				proposedViewName = baseName + "." + idx++.ToString();
			}

			return proposedViewName;
		}

//
//		
//		private string FindNextViewName(string origViewName, string shtNumber)
//		{
//			string proposedViewName;
//
//			int idx = 0;
//			int foundIdx;
//
//			do
//			{
//				proposedViewName = FormatViewName(origViewName, shtNumber, idx);
//				foundIdx         = Array.IndexOf(_viewNameList, proposedViewName, idx++);
//			}
//			while (foundIdx >= 0);
//
//			return proposedViewName;
//
//		}

		// find a ViewSheet based on a Sheet Number
		// returns a ViesSheet if found
		// returns null if not found
		private string FormatViewName(string origViewName, NewSheetFormat nsf)
		{
			if (nsf.NewSheetOption == NewShtOptions.PerSettings)
			{
				return ShNewSheetMgr.FormatShtName(origViewName + " Copy", true,
					nsf.PsNumFmtCode, nsf.seq);
			}

			// new sheet option is from current

			string result = ShNewSheetMgr.FormatShtName(origViewName,
				nsf.FcNameDivChar,
				nsf.FcNameDivCharCustom,
				nsf.FcNameSuffix,
				nsf.FcNameSuffixCustom,
				nsf.seq);

			if ( result.Equals(origViewName))
			{
				result += " Copy";
			}

			return result;

		}

		public int SheetViewExists(string sheetNumber)
		{
			int idx = 0;

			foreach (ViewSheet vs in AllViewSheets())
			{
				if (vs.SheetNumber.Equals(sheetNumber))
				{
					return idx;
				}

				idx++;
			}

			return -1;
		}
		
		// this will get a list of all viewports on a sheet and flag those
		// that are copyable;
		// this must be run in a transaction
		public List<ShViewport> GetCopyableViewports(ViewSheet vs)
		{
			ViewSheet vsDestinationSheet = ViewSheet.Create(_doc, ElementId.InvalidElementId);

			List<ShViewport> vpList = new List<ShViewport>(10);

			// first, get a collection of elements that exist on the source sheet
			FilteredElementCollector colViewSheet =
				new FilteredElementCollector(_doc, vs.Id);

			foreach (Element sourceElem in colViewSheet)
			{
				// we cannot copy some viewports or GuideGrids
				if (sourceElem.Category.Id == _cats.get_Item(BuiltInCategory.OST_Viewports).Id)
				{
					Viewport vp = (Viewport) sourceElem;
					View     vw = (View) _doc.GetElement(vp.ViewId);
					ShViewport svp = new ShViewport();

					svp.Viewport = vp;
					svp.IsCopyable = Viewport.CanAddViewToSheet(_doc, vsDestinationSheet.Id, vw.Id);
					svp.NewViewName = null;

					vpList.Add(svp);
				}
			}

			_doc.Delete(vsDestinationSheet.Id);

			return vpList;
		}

		private void GetAllSheetViews()
		{
			_vs = new FilteredElementCollector(_doc).OfClass(typeof(ViewSheet));
		}

		// create a sorted list of sheet numbers
		private void GetAllSheetNumbers()
		{
			int i = 0;

			GetAllSheetViews();

			_sheetNumberList = new string[SheetCount];

			foreach (ViewSheet vs in _vs)
			{
				_sheetNumberList[i++] = vs.SheetNumber.ToLower();
			}

			Array.Sort(_sheetNumberList);
		}

		// get the names for all of the views in the model - why??
		private void GetAllViewNames()
		{
			int i = 0;

			_mVw = new FilteredElementCollector(_doc).OfClass(typeof (View));

			_viewNameList = new string[_mVw.Count()];

			foreach (View vw in _mVw)
			{
//				_viewNameList[i++] = vw.ViewName;
				_viewNameList[i++] = vw.Name;
			}
		}

		private string GetNewSheetName(NewSheetFormat nsf)
		{
			switch (nsf.NewSheetOption)
			{
				case NewShtOptions.PerSettings:
				{
					return ShNewSheetMgr.FormatShtName(
						nsf.PsShtNamePrefix,
						nsf.PsIncShtName,
						nsf.PsNumFmtCode,
						nsf.seq);
				}
				case NewShtOptions.FromCurrent:
				{
					return ShNewSheetMgr.FormatShtName(
						nsf.FcSelShtName,
						nsf.FcNameDivChar,
						nsf.FcNameDivCharCustom,
						nsf.FcNameSuffix,
						nsf.FcNameSuffixCustom,
						nsf.seq);
				}
			}

			return AppStrings.R_CannotNameSheet;
		}

		private FamilySymbol GetTitleBlockFs(string tb)
		{
			string familyName;
			string typeName;

			char[] dl = { TBlkSeperator };

			FamilySymbol fsFamilySymbol = null;

			string[] names = tb.Split(dl);

			familyName = names[0].Trim();
			typeName   = names[2].Trim();

			FilteredElementCollector tbCol = GetAllTitleBlocks();

			var query = from element in tbCol
				where ((FamilySymbol) element).Family.Name == familyName
					&& ((FamilySymbol) element).Name       == typeName
				select element;

			if (query.Any())
			{
				fsFamilySymbol = (FamilySymbol) query.First();
			}

			return fsFamilySymbol;
		}

		// filter through a ViewSheet to get the title block's family instance
		private FamilyInstance GetTitleBlockFiFromSheet(ViewSheet vs)
		{
			// this gets a collection of elements that are of the
			// titleblock category - this will be 0 or more elements
			FilteredElementCollector colViewSheet
				= new FilteredElementCollector(_doc, vs.Id)
					.OfCategory(BuiltInCategory.OST_TitleBlocks);

			// if none - return the null value
			if (colViewSheet.ToElementIds().Count == 0)
			{
				return null;
			}

			return (FamilyInstance) colViewSheet.FirstElement();
		}

		public string GetTitleBlockFromSheet(ViewSheet vs)
		{
			string titleBlock = null;

			FamilyInstance tbInstance = GetTitleBlockFiFromSheet(vs);

			if (tbInstance != null)
			{
				titleBlock = FormatTitleBlockName(tbInstance.Symbol);
			}

			return titleBlock;
		}

		private void ConfigureNewTitleBlock(Element sourceTBlk, ViewSheet vsDestination)
		{
			// relocate the new title block to match the location of the original
			// title block - get the Instance of the new title block
			FamilyInstance tbInstance = GetTitleBlockFiFromSheet(vsDestination);

			// if the Instance is null - there is no title block to move
			// skip this part
			if (tbInstance != null)
			{
				// get the location of the original title block
				LocationPoint locSource = sourceTBlk.Location as LocationPoint;

				// get the location of the new title block
				LocationPoint locDestination = tbInstance.Location as LocationPoint;

				// calculate the move vector
				XYZ xyzVector = new XYZ(locSource.Point.X - locDestination.Point.X,
					locSource.Point.Y - locDestination.Point.Y, 0);

				// move the title block - if this fails - don't care
				tbInstance.Location.Move(xyzVector);

				// copy the titleblock's parameters
				CopyParameters(sourceTBlk, _doc.GetElement(tbInstance.Id));
			}
		}

		private SketchPlane CreateSketchPlane(ViewSheet vsDestination)
		{
			XYZ ptStart = new XYZ(0, 0, 0);
			XYZ ptEnd = new XYZ(3, 3, 0);

			// create a "line" between these points
			Line line = Line.CreateBound(ptStart, ptEnd);

			XYZ ptOrigin = new XYZ(0, 0, 0);
			XYZ ptNormal = new XYZ(1, 1, 0);

			// create a new "plane"
			Plane plane = Plane.CreateByNormalAndOrigin(ptNormal, ptOrigin);

			// create a new sketchplane in the document
			SketchPlane sketch = SketchPlane.Create(_doc, plane);

			// create a detail "line" in the destination ViewSheet - this creates the new SketchPlane
			// in the ViewSheet
			DetailLine tempDetailLine = _doc.Create.NewDetailCurve(vsDestination, line) as DetailLine;

			// the detail line created was just to create the SketchPlane in the ViewSheet
			// it was temporary - now delete the temporary line
			_doc.Delete(tempDetailLine.Id);

			return sketch;
		}

		private void PlaceViewportOnSheet(ViewSheet vsDestinationSheet, Viewport vpSource, View vw)
		{
			XYZ xyzVpCenter = vpSource.GetBoxCenter();

			Viewport vpDestination = Viewport.Create(_doc, vsDestinationSheet.Id, vw.Id, xyzVpCenter);

			// match the existing rotation
			vpDestination.Rotation = vpSource.Rotation;

			Parameter familySource = vpSource.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM);
			Parameter typeSource = vpSource.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM);

			Parameter familyDistination = vpDestination.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM);
			Parameter typeDetsination = vpDestination.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM);

			if (familySource != null && typeSource != null)
			{
				familyDistination.Set(familySource.AsElementId());
				typeDetsination.Set(typeSource.AsElementId());
			}
		}

		public string SelectTitleBlock(NewSheetFormat nsf)
		{
			string result = nsf.TitleBlockName;

			if (nsf.TitleBlockName.Equals(AppStrings.R_TBlkFromSelSheet))
			{
				result = GetTitleBlockFromSheet(nsf.FcSelViewSht);
			}
			else if (nsf.TitleBlockName.Equals(AppStrings.R_TBlkNone))
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// Copies the parameters from a source Element to a destination Element
		/// </summary>
		/// <param name="elSource">The source Element</param>
		/// <param name="elDestination">The destination Element</param>
		private  void CopyParameters(Element elSource, Element elDestination)
		{
			Parameter pDestination;

			foreach (Parameter pSource in elSource.Parameters)
			{
				pDestination = elDestination.get_Parameter(pSource.Definition);

				if (pDestination != null 
					&& !pSource.IsReadOnly 
					&& pSource.HasValue
					&& !pSource.Definition.Name.Equals(AppStrings.R_SheetNum)
					&& !pSource.Definition.Name.Equals(AppStrings.R_SheetName))
				{
					switch (pSource.StorageType)
					{
						case StorageType.Double:
							pDestination.Set(pSource.AsDouble());
							break;
						case StorageType.Integer:
							pDestination.Set(pSource.AsInteger());
							break;
						case StorageType.String:
							pDestination.Set(pSource.AsString());
							break;
						// ignore the storage types: ElementId and None
					}
				}
			}
		}

		#endregion

		#region + Debug

#if DEBUG

		private void PrintVPParameters(Viewport vp)
		{
			logMsgLn2(nl + "Print Parameter set");

			PrintParameters(vp.Parameters);
			
			logMsgLn2(nl + "Print Parameter map");

			PrintParameters(vp.ParametersMap);
			
			logMsgLn2(nl + "Print Ordered Parameters");

			PrintParameters(vp.GetOrderedParameters());

		}


		// attempt to find and then move the viewport title
		// nothing worked
		private void GetParameterNone(ParameterSet ps)
		{
			foreach (Parameter p in ps)
			{
				if (p.Definition.Name.Equals("None"))
				{
					Element e = _doc.GetElement(p.AsElementId());

					Plane pl = ((SketchPlane) e).GetPlane();

					e.Location.Move(new XYZ(1.0, 1.0, 0));


					break;
				}
			}
		}



		private void PrintParameters(ParameterSet ps)
		{
			foreach (Parameter p in ps)
			{
				PrintParameterInfo(p);
			}
		}

		private void PrintParameters(ParameterMap pm)
		{
			foreach (Parameter p in pm)
			{
				PrintParameterInfo(p);
			}
		}

		private void PrintParameters(IList<Parameter> ps)
		{
			foreach (Parameter p in ps)
			{
				PrintParameterInfo(p);
			}
		}

		private void PrintParameterInfo(Parameter p)
		{
			Debug.Write("Name: " + p.Definition.Name);
			Debug.Write(" --> RO: " + p.IsReadOnly);
			Debug.Write(" --> HV: " + p.HasValue);
			Debug.Write(" --> Storage type: -->  ");

			switch (p.StorageType)
			{
				case StorageType.Double:
					Debug.Print("Double: -->  " + p.AsValueString());
					break;
				case StorageType.Integer:
					if (p.Definition.ParameterType == ParameterType.YesNo)
					{
						Debug.Write("Yes/No: -->  ");
						if (p.AsInteger() == 0)
						{
							Debug.Print("No");
						}
						else
						{
							Debug.Print("Yes");
						}
					}
					else
					{
						Debug.Print("Integer: -->  " + p.AsInteger());
					}
					break;
				case StorageType.ElementId:
					Debug.Write("Element Id: -->  ");

					if (_doc.GetElement(p.AsElementId()) != null)
					{
						Debug.Print(" -->" + _doc.GetElement(p.AsElementId()).Name);
					}
					else
					{
						Debug.Print("null");
					}

					break;
				case StorageType.String:
					Debug.Print("String: --->  " + p.AsString());
					break;
				default:
					Debug.Print("Unexposed parameter: --->  " + p.ToString());
					break;
			}
		}


#endif
		#endregion

	}
}
