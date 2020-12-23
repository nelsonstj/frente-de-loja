const Alert = ({ titulo, mensagem, onClickYes, onClickNo }) => (
  <div className="modal-content">
    <div className="modal-header">
      <h5 className="text-danger">{titulo}</h5>
    </div>
    <div className="modal-body">
      <h5>{mensagem}</h5>
    </div>
    <div className="modal-footer">
      <button type="button" className="btn btn-secondary" data-dismiss="modal" onClick={onClickYes}>Sim</button>
      <button type="button" className="btn btn-inverse" data-dismiss="modal" onClick={onClickNo}>Não</button>
    </div>
  </div>
)


const Modal = (titulo, mensagem, onClickYes, onClickNo) => {
  return <Alert titulo={titulo} mensagem={mensagem} onClickYes={onClickYes} onClickNo={onClickNo}/>
}

window.Modal = Modal;