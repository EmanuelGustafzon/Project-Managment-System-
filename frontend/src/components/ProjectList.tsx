
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
        setProjectToUpdate(null)
        setShowModal(true)
    }
    const updateProject = (project: IProject) => {
        setProjectDetailsToShow(null);
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
            return;
        }
        setDeleteActionOccured("failed to delete project")
        await new Promise((resolve) => setTimeout(() => {
            setDeleteActionOccured(null);
            resolve(null);
        }, 4000));
    }
    const closeModel = () => {
        setShowModal(false)
        setProjectDetailsToShow(null);
        setProjectToUpdate(null)
    }

    return (
        <div className="overflow-x-auto text-align-center">
            <dialog ref={modalRef} className="modal bg-dark">
                <div className="modal-box">
                    {projectDetailsToShow !== null && <ProjectDetails project={projectDetailsToShow} />}
                    {projectToUpdate !== null && < UpdateProjectForm projectToUpdate={projectToUpdate} />}
                    <div className="modal-action">
                            <button onClick={closeModel} className="btn">Close</button>
                    </div>
                </div>
            </dialog>
            {deleteActionOccured &&
                <div className="toast toast-top toast-start z-10">
                    <div className="alert">
                        <span>{deleteActionOccured}</span>
                    </div>
                </div>
            }
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




