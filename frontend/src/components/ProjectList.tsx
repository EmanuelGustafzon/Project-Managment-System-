
import { IProject } from "../interfaces/IProject";
import useFetch from "../hooks/useFetch";
import { useEffect, useRef, useState } from "react";
import UpdateProjectForm from "./UpdateProjectForm";
import ProjectItem from "./ProjectItem";
import ProjectDetails from "./ProjectDetails";
import { useBaseUrl } from "../contexts/BaseUrlContext";

const ProjectList = () => {
    const { data, setData, loading, error } = useFetch<IProject[] | null>('api/Project');
    const [showModal, setShowModal] = useState(false);
    const [deleteActionOccured, setDeleteActionOccured] = useState<string | null>(null);
    const [projectToUpdate, setProjectToUpdate] = useState<IProject | null>(null);
    const [projectDetailsToShow, setProjectDetailsToShow] = useState<IProject | null>(null);
    const modalRef = useRef<HTMLDialogElement>(null);
    const baseUrl = useBaseUrl()
    useEffect(() => {
        if (showModal) {
            modalRef.current?.showModal();
        } else {
            modalRef.current?.close();
        }
    }, [showModal]);

    const projectDetails = (project: IProject) => {
        setProjectDetailsToShow(project);
        setShowModal(true)
    }
    const updateProject = (project: IProject) => {
        setProjectToUpdate(project)
        setShowModal(true)
    }
    const deleteProject = async (id: number) => {
        const res = await fetch(`${baseUrl}/api/Project/${id}`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json"
            },
        });
        if (res.status === 204) {
            setData((prev) => (prev ? prev.filter((x) => x.id !== id) : []));
            setDeleteActionOccured("Successfully deleted project")
            setShowModal(true)
            return;
        }
        setDeleteActionOccured("failed to delete project")
        setShowModal(true)
    }

    const closeModel = () => {
        setShowModal(false)
        setProjectDetailsToShow(null);
        setProjectToUpdate(null)
        setDeleteActionOccured(null);
    }

    return (
        <div className="overflow-x-auto text-align-center">
            <dialog ref={modalRef} className="modal bg-dark">
                <div className="modal-box">
                    {projectDetailsToShow !== null && <ProjectDetails project={projectDetailsToShow} />}
                    {projectToUpdate !== null && < UpdateProjectForm projectToUpdate={projectToUpdate} />}
                    {deleteActionOccured !== null &&
                        <div>
                            <span>{deleteActionOccured}</span>
                        </div>
                    }
                    <div className="modal-action">
                            <button onClick={closeModel} className="btn">Close</button>
                    </div>
                </div>
            </dialog>
            {error && <p>...</p>}
            {loading && <p>LOADING...</p> }
            { data &&
                <table className="table table-xs table-pin-rows table-pin-cols">
                    <thead>
                        <tr>
                            <td>Project Number</td>
                            <td>Name</td>
                            <td>Start Date</td>
                            <td>End Date</td>
                            <td>Status</td>
                        </tr>
                    </thead>
                    {
                        data && data.map((project: IProject) => {
                            return (
                                <ProjectItem
                                    detailsAction={projectDetails}
                                    deleteAction={deleteProject}
                                    updateAction={updateProject}
                                    key={project.id}
                                    project={project}
                                    />
                            );
                        })
                    }
                </table>
            }
        </div>
    )
}

export default ProjectList;




