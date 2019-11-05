$(document).ready(function () {
    var inserir = false;

    GetAll();

    //evento do botao detalhar veiculos
    $("#tbVeiculo").on('click', '.btnSelect', function () {
        //limpar os campos do modal, antes de preenche-los
        LimparCampos();
        //edição
        inserir = false;
        // get the current row
        var currentRow = $(this).closest("tr");
        var placa = currentRow.find("td:eq(0)").text(); // get current row 1st TD value
        GetID(placa, '');
    });

    //evento do botão buscar placa
    $("#tbVeiculo").on('click', '.btnPesquisar', function () {
        var placa = $('#_placaID').val();
        if (placa != '') {
            //Limpa a tabela
            $('#tbVeiculo tbody').empty();
            //gera tabela com a busca;
            GetID(placa, 'sim');
        } else {
            alert('Favor digitar a placa para pesquisa!');
            $('#_placaID').focus();
        }
    });

    //evento do botao detalhar veiculos
    $("#tbVeiculo").on('click', '.btnExcluir', function () {
        // get the current row
        var currentRow = $(this).closest("tr");
        var placa = currentRow.find("td:eq(0)").text(); // get current row 1st TD value
        bootbox.confirm({
            message: 'Confirma a exclusão do veiculo de placa: <b>' + placa + '</b> ?',
            callback: function (confirmacao) {
                if (confirmacao) {
                    DeleteID(placa);
                    bootbox.alert('Veiculo excluído com sucesso!');
                } else {
                    bootbox.alert('Exclusão cancelada!');
                };
            },
            buttons: {
                cancel: { label: 'Cancelar', className: 'btn-default' },
                confirm: { label: 'EXCLUIR', className: 'btn-danger' }

            }
        });

    });

    //evento do botão buscar todos
    $("#tbVeiculo").on('click', '.btnPesquisarTodos', function () {
        $('#tbVeiculo tbody').empty();
        GetAll();
    });

    //evento do botão incluir novo
    $("#tbVeiculo").on('click', '.btnIncluir', function () {
        //Limpar campos
        LimparCampos();
        inserir = true;
    });

    //chamada do modal para dar uma ajeitada para saber se vai usar ele como editou ou inclusao
    $('#ModalDetalisVeiculo').on('shown.bs.modal', function () {
          if (!(inserir)) {
            $('#_placa').attr("disabled", true);
            $('#_marca').focus();
        } else {
            $('#_placa').focus();
        }
    });

    //evento do botao no modal salvar
    $("#ModalDetalisVeiculo").on('click', '.btSalvar', function () {
        if (Validacoes()) {
            var vendido = false;
            if ($('#_vendido').attr('checked')) {
                vendido = true;
            };

            var objeto = {
                Placa: $('#_placa').val(),
                Marca: $('#_marca').val(),
                Ano: $('#_ano').val(),
                Descricao: $('#_descricao').val(),
                Vendido: vendido,
                Created: moment().format('DD/MM/YYYY, HH:mm:ss'),
                Updated: moment().format('DD/MM/YYYY, HH:mm:ss')
            };

            //chamada do metodo post ou metodo put
            if (inserir) {
                PostNew(objeto);
            } else {
                PUTAtualiza(objeto);
            };
        };
    });
});

function Validacoes() {
    //validações simples
    if ($('#_placa').val() == '') {
        alert('Favor preencher a placa!');
        $('#_placa').focus();
        return false;
    };

    if ($('#_marca').val() == '') {
        alert('Favor preencher a marca!');
        $('#_marca').focus();
        return false;
    };

    if ($('#_ano').val() == '') {
        alert('Favor preencher o ano!');
        $('#_ano').focus();
        return false;
    } else if (!(Number.isInteger(parseInt($('#_ano').val())))) {
        alert('Ano, deve ser um numero valido');
        $('#_ano').val('');
        $('#_ano').focus();
        return false;
    };

    if ($('#_descricao').val() == '') {
        alert('Favor fazer uma breve descricao do veiculo!');
        $('#_descricao').focus();
        return false;
    };

    return true;

}

function PUTAtualiza(objeto) {
    var url = 'http://localhost:63593/api/values';

    $.ajax({
        url: url,
        type: "PUT",  //post, delete, put
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        data: JSON.stringify(objeto),
        success: function (data) {
            //chamar msg de sucesso.
            $('#tbVeiculo tbody').empty();
            GetID(objeto['Placa'], 'sim');
            $('#ModalDetalisVeiculo').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //chamar msg de erro.
        }
    });
}

function DeleteID(placa) {
    var url = 'http://localhost:63593/api/values/' + placa;

    $.ajax({
        url: url,
        type: "DELETE",  //post, delete, put
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            //chamar msg de sucesso.
            $('#tbVeiculo tbody').empty();
            GetAll();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //chamar msg de erro.
        }
    });
}


function PostNew(objeto) {
    var url = 'http://localhost:63593/api/values';

    $.ajax({
        url: url,
        type: "POST",  //post, delete, put
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        data: JSON.stringify(objeto),
        success: function (data) {
            //chamar msg de sucesso.
            $('#tbVeiculo tbody').empty();
            GetID(objeto['Placa'], 'sim');
            $('#ModalDetalisVeiculo').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //chamar msg de erro.
        }
    });
}

function LimparCampos() {
    $('#_placa').val('');
    $('#_placa').attr("disabled", false);
    $('#_marca').val('');
    $('#_ano').val('');
    $('#_descricao').val('');
    $('#_vendido').attr('checked', false);
    $('#_created').text('');
    $('#_updated').text('');
    $('#_publicado').text('');
    $('#_atualizacao').text('');
}

function GetAll() {
    var url = 'http://localhost:63593/api/values';

    $.ajax({
        url: url,
        type: "GET",  //post, delete, put
        dataType: "json",
        async: false,
        success: function (data) {
            $.each(data, function (index, element) {
                //var newRowContent = "<tr><td>" + element.placa + "</td><td>" + element.marca + "</td><td><button class='btn btn-primary btn-sm btnSelect'>Detalhar</button></td></tr>";

                var newRowContent = "<tr><td>" + element.placa + "</td><td>" + element.marca + "</td><td>" + element.ano + "</td><td>" +
                    "<button type='button' class='btn btn-primary btnSelect' data-toggle='modal' data-target='#ModalDetalisVeiculo' >" +
                    "Detalhar</button></td><td>" +
                    "<button type='button' class='btn btn-danger btnExcluir'>Excluir</button></td></tr>";

                $('#tbVeiculo tbody').append(newRowContent);
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}


function GetID(placa, tabela) {

    var url = 'http://localhost:63593/api/values/' + placa;
    var obj = {};
    $.ajax({
        url: url,
        type: "GET", //post, delete, put
        dataType: "json",
        async: false,
        success: function (data) {
            //alert(data.marca);
            $.each(data, function (index, element) {
                // se for busca de detalhes entra
                if (tabela == '') {
                    if ((index != 'vendido') && (index != 'created') && (index != 'updated')) {
                        $('#_' + index).val(element);
                    } else {
                        if (index == 'vendido') {
                            $('#_' + index).attr('checked', false);
                            if (element) {
                                $('#_' + index).attr('checked', true);
                            }
                        } else if ((index == 'created') || (index == 'updated')) {
                            $('#_' + index).text(moment(element).format('DD/MM/YY HH:mm'));
                            if (index == 'created') {
                                $('#_publicado').text('Publicado em:');
                            } else {
                                $('#_atualizacao').text('Atualizado em:');
                            }
                        }
                    }
                } else if (tabela == 'sim') {
                    obj[index] = element;
                }
            });
            if ((tabela == 'sim') && (obj['placa'] != null)) {
                var newRowContent = "<tr><td>" + obj['placa'] + "</td><td>" + obj['marca'] + "</td><td>" + obj['ano'] + "</td><td>" +
                    "<button type='button' class='btn btn-primary btnSelect' data-toggle='modal' data-target='#ModalDetalisVeiculo' >" +
                    "Detalhar</button></td><td>" +
                    "<button type='button' class='btn btn-danger btnExcluir'>Excluir</button></td></tr>";

                $('#tbVeiculo tbody').append(newRowContent);
            } else if ((tabela == 'sim') && (obj['placa'] == null)) {
                var newRowContent = "<tr><td>Não há veiculo com esta placa!</td><td></td><td></td><td>" +
                    "</td><td></td></tr>";
                $('#tbVeiculo tbody').append(newRowContent);
                $('#_placaID').val('');
                $('#_placaID').focus();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}