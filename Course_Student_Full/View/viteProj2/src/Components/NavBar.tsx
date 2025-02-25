import { NavLink } from "react-router-dom";

function NavBar() {
  return (
    <nav className="navbar-container">
      <div className="navbar-group">
        <ul className="navbar">
          <li>
            <NavLink to="/">Home</NavLink>
          </li>
          <li>
            <NavLink to="/students">Students</NavLink>
          </li>
          <li>
            <NavLink to="/courses">Courses</NavLink>
          </li>
        </ul>
      </div>
    </nav>
  );
}

export default NavBar;
