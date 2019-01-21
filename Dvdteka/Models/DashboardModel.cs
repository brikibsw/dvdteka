using System.Collections.Generic;

namespace Dvdteka.Models
{
    public class DashboardModel
    {
        public List<RentViewModel> OpenedRents { get; set; }

        public List<DashboardMemberModel> TopMembers { get; set; }

        public List<DashboardDvdModel> TopDvds { get; set; }
    }
}
