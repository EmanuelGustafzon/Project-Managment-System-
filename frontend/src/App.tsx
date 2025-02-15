
import { useEffect, useState, useRef } from 'react';
import './App.css'
import Navbar from './components/Navbar'
import ProjectForm from './components/ProjectForm'
import ProjectList from './components/ProjectList'

function App() {
    const [showModal, setShowModal] = useState(false);
    const modalRef = useRef<HTMLDialogElement>(null);

    useEffect(() => {
        if (showModal) {
            modalRef.current?.showModal();
        } else {
            modalRef.current?.close();
        }
    }, [showModal]);

  return (
      <div className="mt-0">
          <Navbar createProject={() => setShowModal(true)} /> 
          <ProjectList />
          
          <dialog ref={modalRef} className="modal">
              <div className="modal-box">
                  <ProjectForm />
                  <div className="modal-action">
                      <button onClick={() => setShowModal(false)} className="btn">Close</button>
                  </div>
              </div>
          </dialog>
    </div>
  )
}

export default App
