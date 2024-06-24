using SolverLibrary.Model;

namespace railway_monitor.MVVM.Models.Server {
    public abstract class StationPlanSender {
        public abstract void SendPlan(StationWorkPlan plan);
    }
}
