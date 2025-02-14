import React from "react";
import { Plus } from "lucide-react";
import { Button } from "../../ui/Button";
import { Input } from "../../ui/Input";
import Label from "../../ui/Laybel";

export const TopicForm = ({
  currentTopic,
  setCurrentTopic,
  topics,
  currentTopicIndex,
  onAddTopic,
  onSelectTopic,
  onMoveToExercises,
}) => {
  return (
    <div className="space-y-6">
      <div>
        <Label>Topic Name</Label>
        <div className="flex gap-2">
          <Input
            placeholder="Enter topic name"
            value={currentTopic.topicName}
            onChange={(e) =>
              setCurrentTopic((prev) => ({
                ...prev,
                topicName: e.target.value,
              }))
            }
          />
          <Button onClick={onAddTopic} disabled={!currentTopic.topicName}>
            <Plus className="w-4 h-4 mr-2" />
            Add Topic
          </Button>
        </div>
      </div>

      {topics.length > 0 && (
        <div className="mt-4">
          <h3 className="text-sm font-medium mb-2">Topics:</h3>
          <div className="space-y-2">
            {topics.map((topic, index) => (
              <div
                key={index}
                className={`p-3 rounded-lg flex justify-between items-center cursor-pointer
                  ${
                    currentTopicIndex === index
                      ? "bg-blue-50 border-2 border-blue-500"
                      : "bg-gray-50"
                  }`}
                onClick={() => onSelectTopic(index)}
              >
                <span>{topic.topicName}</span>
                <div className="flex items-center gap-2">
                  <span className="text-sm text-gray-500">
                    {topic.exercises.length} exercises
                  </span>
                  {currentTopicIndex === index && (
                    <Button size="sm" onClick={onMoveToExercises}>
                      Add Exercises
                    </Button>
                  )}
                </div>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
};
