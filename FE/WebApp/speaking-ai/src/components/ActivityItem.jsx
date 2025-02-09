import React from "react";

export const ActivityItem = ({ user, action, time }) => {
  return (
    <div className="flex items-center justify-between p-4 border-b last:border-b-0">
      <div className="flex items-center space-x-4">
        <div className="flex flex-col">
          <span className="font-medium">{user}</span>
          <span className="text-gray-600">{action}</span>
        </div>
      </div>
      <span className="text-sm text-gray-500">{time}</span>
    </div>
  );
};
