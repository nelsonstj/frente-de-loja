var express = require('express');        // call express
var app = express();
var bodyParser = require('body-parser');
var sinesp = require('sinesp-nodejs');

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

var port = process.env.PORT || 9001;        // set our port
var router = express.Router();

router.get('/', function (req, res) {
	if (req.query.placa) {
		sinesp.consultaPlaca(req.query.placa).then(dados => {
			res.json(dados);
		}).catch(err => {
			res.json(err);
		})
	} else {
		res.json("Informe o parâmetro placa.");
	}
});

app.use('/api', router);

app.listen(port);
console.log('Consulta Sinesp online na porta ' + port);