import React, { useState } from "react";
import CourseList from "../../components/course/CourseList/CourseList";
import Modal from "../../components/ui/Modal";
import { CourseWizard } from "../../components/course/CourseWizard/CourseWizard";

const CoursePage = () => {
  const [showCreateModal, setShowCreateModal] = useState(false);

  const handleCourseCreate = (courseData) => {
    console.log("Created course:", courseData);
    setShowCreateModal(false);
    // Thực hiện gọi API tạo khóa học ở đây
  };

  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-800">Course Management</h1>
        <button
          onClick={() => setShowCreateModal(true)}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition-colors"
        >
          Create Course
        </button>
      </div>

      <div className="bg-white rounded-lg shadow-md p-6">
        <CourseList />
      </div>

      <Modal
        isOpen={showCreateModal}
        onClose={() => setShowCreateModal(false)}
        title="Create New Course"
        size="large"
      >
        <CourseWizard onComplete={handleCourseCreate} />
      </Modal>
    </div>
  );
};

export default CoursePage;
