import React from "react";
import { Input } from "../../ui/Input";
import TextArea from "../../ui/TextArea";
import Label from "../../ui/Laybel";
// import { Select } from "../../ui/Select";
import Switch from "../../ui/Switch";

const POINT_OPTIONS = [90, 100, 200, 300];

export const CourseBasicForm = ({ courseData, setCourseData }) => {
  return (
    <div className="space-y-6">
      <div>
        <Label>Course Name</Label>
        <Input
          placeholder="Enter course name"
          value={courseData.courseName}
          onChange={(e) =>
            setCourseData((prev) => ({ ...prev, courseName: e.target.value }))
          }
        />
      </div>

      <div>
        <Label>Description</Label>
        <TextArea
          placeholder="Course description"
          value={courseData.description}
          onChange={(e) =>
            setCourseData((prev) => ({ ...prev, description: e.target.value }))
          }
          className="h-32"
        />
      </div>

      <div>
        <Label>Max Points</Label>
        <select
          className="w-full h-10 px-3 border rounded-md"
          value={courseData.maxPoint}
          onChange={(e) =>
            setCourseData((prev) => ({
              ...prev,
              maxPoint: parseInt(e.target.value),
            }))
          }
        >
          {POINT_OPTIONS.map((points) => (
            <option key={points} value={points}>
              {points} points
            </option>
          ))}
        </select>
      </div>

      <div>
        <Label>Course Level</Label>
        <select
          className="w-full h-10 px-3 border rounded-md"
          value={courseData.levelId}
          onChange={(e) =>
            setCourseData((prev) => ({
              ...prev,
              levelId: parseInt(e.target.value),
            }))
          }
        >
          <option value={1}>Beginner</option>
          <option value={2}>Intermediate</option>
          <option value={3}>Advanced</option>
        </select>
      </div>

      <div className="flex items-center space-x-2">
        <Switch
          checked={courseData.isFree}
          onCheckedChange={(checked) =>
            setCourseData((prev) => ({ ...prev, isFree: checked }))
          }
        />
        <Label>Free Course</Label>
      </div>
    </div>
  );
};
