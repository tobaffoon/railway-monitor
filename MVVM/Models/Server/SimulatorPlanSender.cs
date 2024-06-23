using railway_monitor.Simulator;
using SolverLibrary.Model;

namespace railway_monitor.MVVM.Models.Server {
    public class SimulatorPlanSender : StationPlanSender {
        private RailwaySimulator _railwaySimulator;
        
        public SimulatorPlanSender(RailwaySimulator simulator) {
            _railwaySimulator = simulator;
        }

        public override void SendPlan(StationWorkPlan plan) {
            _railwaySimulator.Plan = plan;
        }
    }
}
