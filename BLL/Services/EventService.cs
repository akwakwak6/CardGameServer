using BLL.Models.PresiModel;

namespace BLL.Services {
    public class EventService {

        internal Action<PresiTableList> tableEvent;

        public void AddTableListener(Action<PresiTableList> cb) {
            tableEvent += cb;
        }

        public void RemoveTableListener(Action<PresiTableList> cb) {
            tableEvent -= cb;
        }

    }
}
