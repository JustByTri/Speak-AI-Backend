import React from "react";
import { Line, Pie } from "react-chartjs-2";

export const ChartSection = ({ lineChartData, pieChartData }) => {
  return (
    <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow">
        <h3 className="text-lg font-semibold mb-4">Performance Overview</h3>
        <div className="h-64">
          <Line data={lineChartData} options={{ maintainAspectRatio: false }} />
        </div>
      </div>

      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow">
        <h3 className="text-lg font-semibold mb-4">Resource Allocation</h3>
        <div className="h-64">
          <Pie data={pieChartData} options={{ maintainAspectRatio: false }} />
        </div>
      </div>
    </div>
  );
};
