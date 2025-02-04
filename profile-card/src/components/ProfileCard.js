import React from "react";
import "../styles/ProfileCard.css";
import "bootstrap/dist/css/bootstrap.min.css";

const ProfileCard = () => {
  const user = {
    name: "Himanshi",
    email: "himanshi@gmail.com",
    aadhar: "1234-5678-9012",
    profilePic:
      "https://e7.pngegg.com/pngimages/364/466/png-clipart-github-website-development-software-developer-programmer-github-horse-mammal.png"
  };

  const handleLogout = () => {
    alert("Logged Out Successfully!");
  };

  return (
    <div className="profile-container"> {/* Wrapper for right alignment */}
      <div className="profile-card">
        <div className="profile-header">
          <img src={user.profilePic} alt="Profile" className="profile-pic" />
          <div className="user-info">
            <h5>Hi, {user.name}</h5>
            <p>
              <strong>Email:</strong> {user.email}
            </p>
            <p>
              <strong>Aadhar-Card:</strong> {user.aadhar}
            </p>
          </div>
        </div>
        <button
          className="btn logout-btn btn-block"
          onClick={handleLogout}
          aria-label="Logout"
        >
          Logout
        </button>
      </div>
    </div>
  );
};

export default ProfileCard;
