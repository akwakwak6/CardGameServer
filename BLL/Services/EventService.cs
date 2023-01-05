using BLL.Models.PresiModel;

namespace BLL.Services {
    public class EventService {

        /*
         Service to save functions in a singleton
         */
        internal Action<PresiTableList> tableEvent;

        public void AddTableListener(Action<PresiTableList> cb) {
            tableEvent += cb;
        }

        public void RemoveTableListener(Action<PresiTableList> cb) {
            tableEvent -= cb;
        }

    }
}
