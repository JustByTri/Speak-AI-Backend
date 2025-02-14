import React, { useState } from "react";

import { ChevronRight, ChevronLeft, Plus, Save } from "lucide-react";
import { StepIndicator } from "./StepIndicator";
import { TopicForm } from "./TopicForm";
import { ExerciseForm } from "./ExerciseForm";
import { Card, CardContent } from "../../ui/card";
import { CourseBasicForm } from "./CourseBasicForm";
import { Button } from "@headlessui/react";

export const CourseWizard = ({ onComplete }) => {
  const [step, setStep] = useState(1);
  const [courseData, setCourseData] = useState({
    courseName: "",
    description: "",
    maxPoint: 90,
    isFree: true,
    levelId: 1,
    topics: [],
  });

  const [topics, setTopics] = useState([]);
  const [currentTopicIndex, setCurrentTopicIndex] = useState(null);
  const [currentTopic, setCurrentTopic] = useState({
    topicName: "",
    exercises: [],
  });

  const [currentExercise, setCurrentExercise] = useState({
    content: "",
  });

  const handleAddTopic = () => {
    if (currentTopic.topicName) {
      setTopics((prev) => [...prev, { ...currentTopic, exercises: [] }]);
      setCurrentTopic({ topicName: "", exercises: [] });
      setCurrentTopicIndex(topics.length);
    }
  };

  const handleAddExercise = () => {
    if (currentExercise.content && currentTopicIndex !== null) {
      const updatedTopics = [...topics];
      updatedTopics[currentTopicIndex].exercises.push({ ...currentExercise });
      setTopics(updatedTopics);
      setCurrentExercise({ content: "" });
    }
  };

  const handleSave = () => {
    const finalCourseData = {
      ...courseData,
      topics: topics,
    };
    onComplete(finalCourseData);
  };

  return (
    <div className="max-w-3xl mx-auto">
      <StepIndicator currentStep={step} />

      <Card>
        <CardContent className="pt-6">
          {step === 1 && (
            <CourseBasicForm
              courseData={courseData}
              setCourseData={setCourseData}
            />
          )}
          {step === 2 && (
            <TopicForm
              currentTopic={currentTopic}
              setCurrentTopic={setCurrentTopic}
              topics={topics}
              currentTopicIndex={currentTopicIndex}
              onAddTopic={handleAddTopic}
              onSelectTopic={setCurrentTopicIndex}
              onMoveToExercises={() => setStep(3)}
            />
          )}
          {step === 3 && (
            <ExerciseForm
              currentTopicIndex={currentTopicIndex}
              topics={topics}
              currentExercise={currentExercise}
              setCurrentExercise={setCurrentExercise}
            />
          )}
        </CardContent>
      </Card>

      <div className="mt-6 flex justify-between">
        <Button
          variant="outline"
          onClick={() => setStep((prev) => prev - 1)}
          disabled={step === 1}
        >
          <ChevronLeft className="w-4 h-4 mr-2" />
          Previous
        </Button>

        <div className="space-x-2">
          {step === 3 && (
            <Button
              onClick={handleAddExercise}
              variant="outline"
              disabled={!currentExercise.content}
            >
              <Plus className="w-4 h-4 mr-2" />
              Add Exercise
            </Button>
          )}

          {step < 3 ? (
            <Button onClick={() => setStep((prev) => prev + 1)}>
              Next
              <ChevronRight className="w-4 h-4 ml-2" />
            </Button>
          ) : (
            <Button onClick={handleSave}>
              <Save className="w-4 h-4 mr-2" />
              Save Course
            </Button>
          )}
        </div>
      </div>
    </div>
  );
};
