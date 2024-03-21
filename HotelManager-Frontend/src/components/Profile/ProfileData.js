export const ProfileData = ({ user }) => {
  const userArr = Object.entries(user).slice(1);
  const handleKeyString = (key) => {
    key = key.toUpperCase();
    if (key === "FIRSTNAME" || key === "LASTNAME") {
      const index = key.indexOf("N");
      key = key.slice(0, index) + " " + key.slice(index);
    }
    return key;
  };

  return (
    <div className="profile-data">
      {userArr.map(([key, value]) => {
        return (
          <div className="profile-data-field" key={key}>
            <span className="profile-data-field-heading">
              {handleKeyString(key) + ": "}
            </span>
            <span className="profile-data-field-text">{value}</span>
          </div>
        );
      })}
    </div>
  );
};
