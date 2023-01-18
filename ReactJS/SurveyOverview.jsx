import React, { Fragment, useState, useEffect, useCallback } from "react";
import {
  Card,
  Col,
  Container,
  Row,
  Table,
  Breadcrumb,
  Button,
  Offcanvas,
} from "react-bootstrap";
import PropTypes from "prop-types";
import toastr from "toastr";
import surveyQuestionsService from "services/surveyQuestionsService";
import SingleSurveyTable from "./SingleSurveyTable";
import lookUpService from "services/lookUpService";
import OffCanvasSurveyAdd from "./OffCanvasSurveyAdd";
import SurveyModalForm from "./SurveyModalForm";
import swal from "sweetalert";
import "./surveybuilder.css";
import debug from "sabio-debug";

const _logger = debug.extend("SurveyOverview");
const SurveyOverview = ({ currentUser }) => {
  const [pageData] = useState({
    pageIndex: 0,
    pageSize: 50,
    userId: currentUser.id,
  });
  const [surveys, setSurveys] = useState({
    arrayOfSurveys: [],
    surveyComponents: [],
  });
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    surveyTypeId: "",
    statusId: "",
  });

  useEffect(() => {
    surveyQuestionsService
      .getAllSurveyDetailedPaginated(pageData.pageIndex, pageData.pageSize)
      .then(onGetSurveyDetailedSuccess)
      .catch(onGetSurveyError);
    lookUpService
      .LookUp(["SurveyTypes", "SurveyStatus"])
      .then(onGetLookUpSuccess)
      .catch(onGetLookUpError);
  }, []);

  const [surveyTypes, setSurveyTypes] = useState([{ name: "", id: "" }]);
  const [surveyStatus, setSurveyStatus] = useState([{ name: "", id: "" }]);
  const onGetLookUpSuccess = (response) => {
    let newTypes = response?.item.surveyTypes;
    let newStatuses = response?.item.surveyStatus;
    setSurveyTypes(newTypes);
    setSurveyStatus(newStatuses);
  };
  const onGetLookUpError = () => {
    toastr["error"]("Failed to get look ups.", "Error");
  };
  const mapSurveyTypes = surveyTypes.map((surveyType) => (
    <option value={surveyType.id} key={surveyType.id}>
      {surveyType.name}
    </option>
  ));
  const mapSurveyStatus = surveyStatus.map((surveyStatus) => (
    <option value={surveyStatus.id} key={surveyStatus.id}>
      {surveyStatus.name}
    </option>
  ));
  const onGetSurveyDetailedSuccess = (response) => {
    let getSurveyDetails = response.item.pagedItems;
    setSurveys((prevState) => {
      const pd = { ...prevState };
      pd.arrayOfSurveys = getSurveyDetails;
      pd.surveyComponents = getSurveyDetails.map(mapSurveys);
      return pd;
    });
  };
  const onGetSurveyError = () => {
    toastr["error"]("Failed to load surveys.", "Error");
  };

  const onDeleteRequested = useCallback((survey) => {
    swal({
      title: "Are you sure?",
      text: "Once deleted, you will not be able to recover this survey!",
      icon: "warning",
      buttons: ["Nevermind!", "Delete!"],
      dangerMode: true,
    }).then(() => {
      let questions = survey.surveyQuestions?.length;
      if (survey.status.id === 1) {
        return toastr["warning"]("Survey cannot be active.", "Delete Failed");
      } else if (questions > 0) {
        return toastr["warning"](
          "Survey cannot have questions.",
          "Delete Failed"
        );
      } else {
        swal("The survey was permanently deleted!", {
          icon: "success",
        });
        deleteConfirmed();
      }
    });
    const deleteConfirmed = () => {
      surveyQuestionsService
        .deleteSurveyOnly(survey.id)
        .then(callbackDeleteSuccessHandler(survey))
        .catch(callbackErrorHandler);
    };
  }, []);
  const callbackDeleteSuccessHandler = (aSurvey) => {
    const surveyToRemove = aSurvey.id;
    setSurveys((prevState) => {
      const pd = { ...prevState };
      pd.arrayOfSurveys = [...pd.arrayOfSurveys];
      const idxOfSurvey = pd.arrayOfSurveys.findIndex((aSurvey) => {
        let result = false;
        if (aSurvey.id === surveyToRemove) {
          result = true;
        }
        return result;
      });
      if (idxOfSurvey >= 0) {
        pd.arrayOfSurveys.splice(idxOfSurvey, 1);
        pd.surveyComponents = pd.surveyComponents =
          pd.arrayOfSurveys.map(mapSurveys);
      }
      return pd;
    });
  };
  const callbackErrorHandler = (response) => {
    _logger("Error", response);
    swal({
      title: "ERROR",
      text: "An error occured and the survey was unchanged.",
      icon: "error",
      button: "Ok",
    });
  };
  const mapSurveys = (survey) => {
    return (
      <SingleSurveyTable
        key={survey.id}
        survey={survey}
        onDelete={onDeleteRequested}
        onEdit={handleEditSurveyForm}
        badgeColor={badgeColour}
      ></SingleSurveyTable>
    );
  };
  //// Modal & OffCanvas
  const [show, setShow] = useState(false);
  const handleShow = () => setShow(true);
  const handleClose = () => setShow(false);
  const [show2, setShow2] = useState(false);
  const handleShow2 = () => setShow2(true);
  const handleClose2 = () => setShow2(false);
  ////
  const handleEditSurveyForm = (survey) => {
    handleShow();
    setFormData({ surveyId: survey.id });
    const formValues = {
      id: survey.id,
      name: survey.name,
      description: survey.description,
      surveyTypeId: survey.surveyType.id,
      statusId: survey.status.id,
    };
    setFormData(formValues);
  };
  const onModalEditSubmit = (values) => {
    surveyQuestionsService
      .editTitle(values.id, values)
      .then(callbackUpdateSuccessHandler(values))
      .catch(callbackErrorHandler);
  };
  const callbackUpdateSuccessHandler = (aSurvey) => {
    const surveyToUpdate = aSurvey.id;
    setSurveys((prevState) => {
      const pd = { ...prevState };
      pd.arrayOfSurveys = [...pd.arrayOfSurveys];
      const idxOfSurvey = pd.arrayOfSurveys.findIndex((aSurvey) => {
        let result = false;
        if (aSurvey.id === surveyToUpdate) {
          result = true;
        }
        return result;
      });
      if (idxOfSurvey >= 0) {
        let updatedSurvey = pd.arrayOfSurveys[idxOfSurvey];
        updatedSurvey.name = aSurvey.name;
        updatedSurvey.description = aSurvey.description;
        updatedSurvey.surveyType.id = aSurvey.surveyTypeId;
        updatedSurvey.surveyType.name = surveyTypes.find(
          (type) => type.id === parseInt(aSurvey.surveyTypeId)
        )?.name;
        updatedSurvey.status.id = aSurvey.statusId;
        updatedSurvey.status.name = surveyStatus.find(
          (type) => type.id === parseInt(aSurvey.statusId)
        )?.name;
        pd.arrayOfSurveys[idxOfSurvey] = updatedSurvey;
        let movingArr = pd.arrayOfSurveys.splice(idxOfSurvey, 1);
        let splicedAgain = movingArr.splice();
        splicedAgain.push(movingArr);
        pd.arrayOfSurveys.unshift(...movingArr);
        pd.surveyComponents = pd.arrayOfSurveys.map(mapSurveys);
        handleClose();
      }
      return pd;
    });
  };

  function badgeColour(value) {
    const surveyStatuses = parseInt(value);
    switch (surveyStatuses) {
      case 1:
        return "badge bg-success";
      case 2:
        return "badge bg-secondary";
      case 3:
        return "badge bg-warning";
      case 4:
        return "badge bg-danger";
      default:
        return "badge bg-primary";
    }
  }

  return (
    <Fragment>
      <div className="py-4 py-lg-6 bg-colors-gradient d-lg-flex align-items-center justify-content-between">
        <Container>
          <Row>
            <Col lg={{ span: 11, offset: 1 }} md={12} sm={12}>
              <div className="mb-4 mb-lg-0">
                <h1 className="text-black mb-1">Survey Overview</h1>
                <p className="mb-0 text-black lead">
                  A quick overview of all surveys provided.
                </p>
              </div>
            </Col>
          </Row>
        </Container>
      </div>
      <Card className="border-0">
        <Card.Body>
          <div>
            <Row>
              <Row>
                <Col lg={12} md={12} sm={12}>
                  <div className="border-bottom pb-4 d-md-flex align-items-center justify-content-between">
                    <Breadcrumb>
                      <Breadcrumb.Item href="/dashboard/admin">
                        Dashboard
                      </Breadcrumb.Item>
                      <Breadcrumb.Item active>Survey Overview</Breadcrumb.Item>
                    </Breadcrumb>
                    <div>
                      <Button onClick={handleShow2}>Create a Survey</Button>
                    </div>
                  </div>
                </Col>
              </Row>
            </Row>
          </div>
        </Card.Body>
        {/*Table*/}
        <Table responsive hover className="table-primary">
          <thead>
            <tr>
              <th>SURVEY</th>
              <th>DESCRIPTION</th>
              <th>QUESTIONS</th>
              <th>CREATED ON</th>
              <th>MODIFIED ON</th>
              <th>AUTHOR</th>
              <th>TYPE</th>
              <th>STATUS</th>
              <th>ACTIONS</th>
            </tr>
          </thead>
          <tbody>{surveys.surveyComponents}</tbody>
        </Table>
      </Card>
      {/*Edit Service Modal */}
      <SurveyModalForm
        show={show}
        onHide={handleClose}
        onSubmit={onModalEditSubmit}
        formData={formData}
        mapSurveyTypes={mapSurveyTypes}
        mapSurveyStatus={mapSurveyStatus}
      ></SurveyModalForm>
      <Offcanvas
        show={show2}
        onHide={handleClose2}
        placement="end"
        name="end"
        className="offcanvasadd-width"
      >
        <Offcanvas.Header closeButton>
          <Offcanvas.Title as="h3">Create Survey</Offcanvas.Title>
        </Offcanvas.Header>
        <Offcanvas.Body className="pt-0">
          <OffCanvasSurveyAdd
            onClick={handleClose2}
            mapSurveyTypes={mapSurveyTypes}
            mapSurveyStatus={mapSurveyStatus}
          />
        </Offcanvas.Body>
      </Offcanvas>
    </Fragment>
  );
};
SurveyOverview.propTypes = {
  currentUser: PropTypes.shape({
    email: PropTypes.string,
    id: PropTypes.number,
    isLoggedIn: PropTypes.bool,
    roles: PropTypes.arrayOf(String),
  }),
};

export default SurveyOverview;
