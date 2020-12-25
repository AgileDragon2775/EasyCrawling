using EasyCrawling.Views;
using System.ComponentModel;

namespace EasyCrawling.Enums
{
    public enum EncodingOptionType
    {      
        [Description("No")]
        NOTTING,
        [Description("Add using word")]
        ADD_WORD,
        [Description("Add using Numbering")]
        ADD_NUMBERING,
        [Description("Add using Name")]
        ADD_NAME,     
        [Description("Cut using word")]
        CUT_WORD,
        [Description("Cut until word")]
        CUT_UNTIL_WORD,
        [Description("Cut using position")]
        CUT_POSITION
    }

    public enum VisualType
    {
        [Description("Title")]
        TITLE,
        [Description("Detail Text")]
        DETAIL,
        [Description("Logo Image")]
        LOGO_IMG,
        [Description("Main Image")]
        MAIN_IMG,
        [Description("Button1")]
        BUTTON1,
        [Description("Button2")]
        BUTTON2,
        [Description("Button3")]
        BUTTON3
    }

    public enum WhenType
    {
        [Description("Body (When click body)")]
        BODY,
        [Description("Button1 (When click Button1)")]
        BUTTON1,
        [Description("Button2 (When click Button2)")]
        BUTTON2,
        [Description("Button3 (When click Button3)")]
        BUTTON3,
        [Description("See more (When click \"See more\")")]
        SEE_MORE,
        [Description("Header (When click Header)")]
        HEADER
    }

    public enum BaseActionType
    {
        [Description("Notting")]
        NONE,
        [Description("Notifity")]
        NOTIFITY,
        [Description("Open app")]
        OPEN_APP,
        [Description("Open url")]
        OPEN_URL,
        [Description("Open file")]
        OPEN_FILE,
        [Description("Download file")]
        DOWNLOAD_FILE
    }

    public enum ReturnType
    {
        TEXT,
        CLASS
    }

    public enum MyDoc
    {
        [Description("HTML")]
        [ClassDescription(typeof(HtmlView))]
        HTML_DOC,

        [Description("Original word Tree")]
        [ClassDescription(typeof(OriginalWordTreeView))]
        ORIGINAL_WORD_TREE_DOC,

        [Description("Selected List from Tree")]
        [ClassDescription(typeof(OriginalWordListVIew))]
        ORIGINAL_WORD_LIST_DOC,

        [Description("Custom words")]
        [ClassDescription(typeof(CustomWordView))]
        CUSTOM_WORD_DOC,       

        [Description("Last Action")]
        [ClassDescription(typeof(ActionView))]
        ACTION_DOC,

        [Description("Test")]
        [ClassDescription(typeof(TestView))]
        TEST_DOC,

        [Description("Detail Notification")]
        [ClassDescription(typeof(ToastView))]
        Toast_DOC,      
    }

    public enum TimeSpanType
    {
        [Description("No")]
        NONE,
        [Description("One time per few minutes")]   //몇분마다
        PER_FEW_MINUTES,
        [Description("Every time run this program")]   //프로그램 킬때마다
        BOOT_PROGRAM,
        [Description("Specific time of the day and when running this program")]   //정해진 시간에 컴퓨터 켜져있을때
        SPECIFIC_TIME,
        [Description("Specific time of the day FORCE")]   //정해진 시간에 컴퓨터 안켜져 있으면 켜지면 바로
        SPECIFIC_TIME_FORCE,
    }

    public enum Week
    {
        [Description("Sunday")]
        SUNDAY,
        [Description("Monday")]
        MONDAY,
        [Description("Tuesday")]
        TUESDAY,
        [Description("Wednesday")]
        WEDNESDAY,
        [Description("Thursday")]
        THURSDAY,
        [Description("Friday")]
        FRIDAY,
        [Description("Saturday")]
        SATURDAY,
        [Description("Notting")]
        NONE,
    }
}