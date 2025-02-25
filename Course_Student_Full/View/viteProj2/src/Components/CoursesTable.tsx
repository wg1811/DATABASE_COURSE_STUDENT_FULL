import { useState } from "react";

interface Student {
  id: number;
  name: string;
  age: number;
  email: string;
  courses: Course[];
}

interface Course {
  name: string;
  students: Student[];
}

const CoursesTable = () => {
  const [courses, setCourses] = useState<Course[]>([]);

  const getCourses = async () => {
    const response = await fetch("http://localhost:5024/courses/getallcourse");
    const data = await response.json();
    setCourses(data);
    console.log(JSON.stringify(data, null, 2));
  };

  return (
    <div className="table-container">
      <button className="fetch-button" onClick={getCourses}>
        Get Courses
      </button>
      <table className="student-table">
        <thead>
          <tr>
            <th>Name</th>

            <th>Students</th>
          </tr>
        </thead>
        <tbody>
          {courses.map((course) => (
            <tr key={course.name}>
              <td>{course.name}</td>
              <td>
                <div className="course-list">
                  {course.students.map((student, index) => (
                    <span key={index} className="courses-tag">
                      {student.name}
                    </span>
                  ))}
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default CoursesTable;
