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
}

const StudentTable = () => {
  const [students, setStudents] = useState<Student[]>([]);

  const getStudents = async () => {
    const response = await fetch(
      "http://localhost:5024/students/getallstudents"
    );
    const data = await response.json();
    setStudents(data);
    console.log(JSON.stringify(data, null, 2));
  };

  return (
    <div className="table-container">
      <button className="fetch-button" onClick={getStudents}>
        Get Students
      </button>
      <table className="student-table">
        <thead>
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Age</th>
            <th>Email</th>
            <th>Courses</th>
          </tr>
        </thead>
        <tbody>
          {students.map((person) => (
            <tr key={person.id}>
              <td>{person.id}</td>
              <td>{person.name}</td>
              <td>{person.age}</td>
              <td>{person.email}</td>
              <td>
                <div className="course-list">
                  {person.courses.map((course, index) => (
                    <span key={index} className="course-tag">
                      {course.name}
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

export default StudentTable;
