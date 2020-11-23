using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SETSReport.Controllers
{
    public static class GlobalVar
    {
        public static readonly String RankQuery = "SELECT DISTINCT PositionID, RankName Name FROM view_FullExamineeResults ORDER BY RankName ASC";
        public static readonly String TestNameDateQuery = "SELECT DISTINCT TestID, TestName, DateCreated, TestNameDate FROM view_FullExamineeResults ORDER BY TestName ASC, DateCreated DESC";
        public static readonly String NationalityQuery = "SELECT PKey, Nat FROM view_AllTestNationality ORDER BY Nat ASC";
        public static readonly String TestNameQuery = "SELECT DISTINCT TestName FROM view_FullExamineeResults ORDER BY TestName ASC";
        public static readonly String SubjNameQuery = "SELECT DISTINCT s.SubjectID, SubjectName FROM tblActualTestDetail atd INNER JOIN tblAdmSubject s ON atd.SubjectID=s.SubjectID ORDER BY SubjectName ASC";
        public static readonly String SubjCategoryQuery = "SELECT * FROM tblAdmSubjectCategory ORDER BY SubjCategoryName ASC";
        public static readonly String SubjGroupQuery = "SELECT * FROM tblAdmSubjectGroup ORDER BY SubjGroupName ASC";
        public static readonly String SubjLevelQuery = "SELECT * FROM tblAdmSubjectLevel ORDER BY SubjLevelName ASC";
        public static readonly String TestGroupQuery = "SELECT * FROM tblAdmTestGroup ORDER BY ID ASC";
        public static readonly String VesselTypeQuery = "SELECT DISTINCT vt.VesselTypeID, VesselType FROM tblAdmTestVslTypeTag vtt INNER JOIN tblAdmVslType vt ON vt.VesselTypeID=vtt.VesselTypeID ORDER BY VesselType ASC";
        public static readonly String SignatoryQuery = "SELECT SignatoryID, DisplayAs, JobTitle, CONCAT(DisplayAs, ' - ', JobTitle) DisplayValue FROM tblAdmSignatory ORDER BY DisplayValue ASC";
        public static readonly String CompanyNameQuery = "SELECT DISTINCT CompanyName FROM view_FullExamineeResults ORDER BY CompanyName ASC";
        public static readonly String SiteNameQuery = "SELECT DISTINCT SiteName FROM tblsites ORDER BY SiteName ASC"; //"SELECT DISTINCT SiteName FROM view_FullExamineeResults ORDER BY SiteName ASC";

        public static String SiteID;
    }


}