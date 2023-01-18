import React from "react";
import { Badge, Dropdown, Image } from "react-bootstrap";
import { FiTrash2, FiEdit, FiTool } from "react-icons/fi";
import PropTypes from "prop-types";
import { formatDateTime } from "../../utils/dateFormater";
import { useNavigate } from "react-router-dom";
import debug from "sabio-debug";

const _logger = debug.extend("SurveyTable");
function SingleSurveyTable(props) {
  let survey = props.survey;
  let author = props.survey.createdBy;
  let handleDelete = props.onDelete;
  let handleEdit = props.onEdit;
  let badgeColour = props.badgeColor;

  const navigate = useNavigate();
  const navigateToEdit = () => {
    const questionsToMap = survey.surveyQuestions;
    let mappedQuestions = questionsToMap.map((question) => ({
      ...question,
      questionTypeId: question.questionType.id,
      question: question.text,
      statusId: survey.status.id,
    }));
    let surveyQuestions = mappedQuestions.forEach((object) => {
      delete object["questionType"];
      delete object["text"];
    });

    let modifiedSurvey = survey;
    modifiedSurvey.statusId = survey.status.id;
    modifiedSurvey.surveyTypeId = survey.surveyType.id;
    delete modifiedSurvey.createdBy;
    delete modifiedSurvey.dateModified;
    delete modifiedSurvey.dateCreated;
    delete modifiedSurvey.status;
    delete modifiedSurvey.surveyType;

    _logger("deleted objects =", surveyQuestions);
    const surveyState = {
      type: "SurveyBuilder_edit",
      payload: {
        ...survey,
        surveyQuestions: mappedQuestions,
      },
    };
    navigate(`/dashboard/surveys/${survey.id}/edit/`, { state: surveyState });
  };

  const questionsCount = () => {
    const question = survey?.surveyQuestions;
    if (question === null) {
      return "0";
    } else return parseInt(question.length);
  };

  return (
    <React.Fragment>
      <tr key={survey.id}>
        <td>{survey.name}</td>
        <td>{survey.description}</td>
        <td>{questionsCount()}</td>
        <td>{formatDateTime(survey.dateCreated)}</td>
        <td>{formatDateTime(survey.dateModified)}</td>
        <td>
          <Image
            alt="avatar"
            src={author.avatarUrl}
            className="avatar avatar-xs"
            roundedCircle
          />{" "}
          {author.firstName} {author.lastName}
        </td>
        <td>{survey.surveyType.name}</td>
        <td>
          <Badge className={badgeColour(survey.status.id)}>
            {survey.status.name}
          </Badge>
        </td>
        <td>
          <Dropdown>
            <Dropdown.Toggle size="sm" />
            <Dropdown.Menu align="end">
              <Dropdown.Item onClick={() => handleEdit(survey)}>
                <FiEdit size="18px" className="dropdown-item-icon" /> Manage
                Survey
              </Dropdown.Item>
              <Dropdown.Item onClick={() => navigateToEdit(survey)}>
                <FiTool size="18px" className="dropdown-item-icon" /> Manage
                Questions
              </Dropdown.Item>
              <Dropdown.Item onClick={() => handleDelete(survey)}>
                <FiTrash2 size="18px" className="dropdown-item-icon" /> Remove
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </td>
      </tr>
    </React.Fragment>
  );
}
SingleSurveyTable.propTypes = {
  survey: PropTypes.shape({
    id: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    status: PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
      name: PropTypes.string.isRequired,
    }).isRequired,
    surveyType: PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
      name: PropTypes.string.isRequired,
    }).isRequired,
    createdBy: PropTypes.shape({
      firstName: PropTypes.string.isRequired,
      lastName: PropTypes.string.isRequired,
      avatarUrl: PropTypes.string.isRequired,
    }).isRequired,
    surveyQuestions: PropTypes.arrayOf(
      PropTypes.shape({
        id: PropTypes.number.isRequired,
        text: PropTypes.string.isRequired,
        helpText: PropTypes.string.isRequired,
        isRequired: PropTypes.bool.isRequired,
        isMultipleAllowed: PropTypes.bool.isRequired,
        questionType: PropTypes.shape({
          id: PropTypes.number.isRequired,
          name: PropTypes.string.isRequired,
        }).isRequired,
        surveyId: PropTypes.number.isRequired,
        sortOrder: PropTypes.number.isRequired,
      })
    ).isRequired,
    dateCreated: PropTypes.string.isRequired,
    dateModified: PropTypes.string.isRequired,
  }).isRequired,
  onDelete: PropTypes.func.isRequired,
  onEdit: PropTypes.func.isRequired,
  badgeColor: PropTypes.func.isRequired,
};

export default React.memo(SingleSurveyTable);
