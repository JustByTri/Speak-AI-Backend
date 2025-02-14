import React from "react";
import TextArea from "../../ui/TextArea";
import Label from "../../ui/Laybel";

export const ExerciseForm = ({
  currentTopicIndex,
  topics,
  currentExercise,
  setCurrentExercise,
}) => {
  if (currentTopicIndex === null) return null;

  return (
    <div className="space-y-6">
      <div>
        <div className="flex items-center justify-between mb-4">
          <Label>
            Exercise Content for Topic: {topics[currentTopicIndex].topicName}
          </Label>
        </div>
        <TextArea
          placeholder="Enter exercise content"
          value={currentExercise.content}
          onChange={(e) =>
            setCurrentExercise((prev) => ({ ...prev, content: e.target.value }))
          }
          className="h-32"
        />
      </div>

      {topics[currentTopicIndex].exercises.length > 0 && (
        <div className="mt-4">
          <h3 className="text-sm font-medium mb-2">Added Exercises:</h3>
          <div className="space-y-2">
            {topics[currentTopicIndex].exercises.map((exercise, index) => (
              <div key={index} className="p-3 bg-gray-50 rounded-lg">
                <p className="text-sm">
                  {exercise.content.substring(0, 100)}...
                </p>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
};
