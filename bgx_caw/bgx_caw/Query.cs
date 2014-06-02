using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw
{
    class Query
    {
        public static String getPotentialNamesFromPageNumber(int number, String id)
        {
            return "SELECT dbo.tblPotential.Name FROM dbo.tblPotential INNER JOIN dbo.tblPage ON dbo.tblPotential.P_id = dbo.tblPage.P_id  INNER JOIN dbo.tblDiagramm ON dbo.tblPage.D_id = dbo.tblDiagramm.D_ID where dbo.tblPage.OriginNumber = '" + number + "' AND dbo.tblDiagramm.D_ID = '"+id+"'";
        }
        public static String getPartNameFromPageNumber(int number)
        {
            return "SELECT dbo.tblPart.BMK" +
            "FROM dbo.tblPart INNER JOIN dbo.tblPage" +
            "ON dbo.tblPart.P_id = dbo.tblPage.P_id  INNER JOIN dbo.tblDiagramm" +
            "ON dbo.tblPage.D_id = dbo.tblDiagramm.D_ID" +
            "WHERE dbo.tblPage.OriginNumber = '" + number + "'";
        }
        public static String getPotentialPagenumbersFromPotentialNames(String name, String id)
        {
            return "SELECT dbo.tblPage.OriginNumber FROM dbo.tblPotential INNER JOIN dbo.tblPage ON dbo.tblPotential.P_id = dbo.tblPage.P_id INNER JOIN dbo.tblDiagramm ON dbo.tblPage.D_id = dbo.tblDiagramm.D_ID WHERE dbo.tblPotential.Name ='" + name + "'AND dbo.tblDiagramm.D_ID = '" + id + "'";        
        }
    }
}
