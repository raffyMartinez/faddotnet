namespace FAD3.Database.Classes
{
    public enum ClassificationType
    {
        None,
        NaturalBreaks,
        JenksFisher,
        UniqueValues,
        EqualIntervals,
        EqualCount,
        StandardDeviation,
        EqualSumOfValues
    }

    public enum ExportImportAction
    {
        ActionExport,
        ActionImport
    }

    public enum PointFeatureType
    {
        MunicipalityLocation,
        BarangayLocation,
        LandmarkLocation,
        IslandLocation
    }

    public enum GridMapSideToPrint
    {
        SideToPrintIgnore,
        SideToPrintFront,
        SideToPrintReverse
    }

    public enum ImportInventoryAction
    {
        ImportDoNothing,
        ImportIntoExisting,
        ImportIntoNew
    }

    public enum OccurenceDataType
    {
        Species,
        Gear
    }

    public enum FisheriesInventoryLevel
    {
        Root,
        Project = 0,
        TargetArea,
        Province,
        Municipality,
        Barangay,
        Sitio,
        FisherVessel,
        Gear
    }

    public enum SubGridType
    {
        Row,
        Column
    }

    public enum DataFileType
    {
        None = 0,
        Text = 1,
        HTML = 2,
        XML = 4,
        Excel = 8,
        CSV = 16
    }

    public enum FileDialogType
    {
        FileOpen,
        FileSave
    }

    public enum ExportImportDataType
    {
        None = 0,
        SpeciesNames = 1,
        GearsVariation = 2,
        GearsLocalName = 4,
        GearsClass = 8,
        LocalNameLanguages = 16,
        CatchLocalNames = 32,
        CatchLocalNameSpeciesNamePair = 64,
        CatchNametDataSelect = 128,
        GearNameDataSelect = 256,
        GearInventory = 512,
        Enumerators = 1024,
        GearInventoryDataSelect = 2048,
        GearsRefCode = 4096,
        CatchNameAll = 8192
    }

    public enum EditActionType
    {
        ActionTypeNone,
        ActionTypeEdit,
        ActionTypeAdd
    }

    public enum CatchNameDataType
    {
        CatchSpeciesName,
        CatchLocalName,
        CatchSpeciesLocalNamePair
    }

    public enum FisheryObjectNameType
    {
        CatchLocalName,
        GearLocalName,
        GearVariationName,
        FishingAccessory,
        FishingExpense
    }

    public enum UIControlType
    {
        TextBox,
        ComboBox,
        Spacer,
        DateMask,
        TimeMask,
        Check
    }

    /// <summary>
    /// categories of corners used in defining extents
    /// </summary>
    public enum fadCornerType
    {
        cornerTypeUndefined,
        cornerTypeUpperLeft,
        cornerTypeLowerRight
    }

    /// <summary>
    /// defines categories of grid types
    /// </summary>
    public enum fadGridType
    {
        gridTypeNone,
        gridTypeGrid25,
        gridTypeOther
    }

    /// <summary>
    /// UTM zones that completely covers fishing grounds
    /// </summary>
    public enum fadUTMZone
    {
        utmZone_Undefined,
        utmZone50N,
        utmZone51N
    }

    /// <summary>
    /// categories of subgridding minor grids for finer resolution of fishing grounds
    /// </summary>
    public enum fadSubgridSyle
    {
        SubgridStyleNone,
        SubgridStyle4,
        SubgridStyle9
    }

    //this is a simplistic list of taxonomic categories
    //This list exists because it is needed for gonad maturity categories
    //and gonad maturity stages varies by taxonomies.
    //
    //No new categories should be added to the database if it is
    //not included in this enumeration
    public enum Taxa
    {
        To_be_determined,
        Fish,
        Shrimps,
        Cephalopods,
        Crabs,
        Shells,
        Lobsters,
        Sea_cucumbers,
        Sea_urchins,
    }

    public enum Identification
    {
        Scientific = 1,
        LocalName
    }

    public enum FishCrabGMS
    {
        AllTaxaNotDetermined,
        FishJuvenile = 1,
        FishStg1Immature,
        FishStg2Maturing,
        FishStg3Mature,
        FishStg4Gravid,
        FishStg5Spent,
        FemaleCrabImmature = 2,
        FemaleCrabMature = 4,
        FemaleCrabBerried,
    }

    public enum Sex
    {
        Juvenile,
        Male,
        Female
    }

    public enum fad3DataStatus
    {
        statusFromDB,
        statusNew,
        statusEdited,
        statusForDeletion
    }

    public enum fad3MappingMode
    {
        defaultMode,
        grid25Mode,
        thematicPointMode,
        fishingGroundMappingMode,
        occurenceMappingGear,
        occurenceMappingSpecies,
        occurenceMappingGearAggregated,
        occurenceMappingSpeciesAggregated,
        effortMappingAggregated,
        efforMapping
    }

    public enum ExtentCompare
    {
        excoSimilar,
        excoOutside,
        excoInside,
        excoCrossing
    }

    public enum fad3CatchSubRow
    {
        none,
        LF,
        GMS
    }

    public enum fad3ActionType
    {
        atIgnore,
        atTakeNote,
        atRemove
    }

    public enum fad3GearEditAction
    {
        addAOI,
        addLocalName,
        addGearCode,
        addGearVariation,
        editLocalName,
        editGearVariation,
        editGearCode
    }

    public enum lvContext
    {
        None = 0,
        CatchAndEffort,
        CatchComposition,
        EnumeratorSampling,
        LengthFreq,
        GMS
    }

    public enum CatchMeristics
    {
        LengtFreq,
        LengthWeightSexMaturity,
        LenghtWeightSex,
    }

    public enum CoordinateDisplayFormat
    {
        DegreeDecimal,
        DegreeMinute,
        DegreeMinuteSecond,
        UTM
    }

    //public static class Enums
    //{
    //}
}