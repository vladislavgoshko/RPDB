//Функция получения данных и занесения в таблицу 
async function GetAllOrders() {
	const response = await fetch("/api/orders", {
		method: "GET",
		headers: { "Accept": "application/json" }
	});

	if (response.ok === true) {
		const orders = await response.json();
		let rows = document.querySelector("tbody");

		orders["$values"].forEach(order => {
			rows.append(row(order));
		});
	}
}

async function GetAllProducts() {
	var selectList = orderForm.product;
	const response = await fetch("/api/products", {
		method: "GET",
		headers: { "Accept": "application/json" }
	});
	if (response.ok === true) {
		const products = await response.json();
		products["$values"].forEach(product => {
			var option = document.createElement("option");
			option.text = product.name;
			option.value = parseInt(product.id);
			selectList.append(option)
		});
	}
}

async function GetAllCustomers() {
	var selectList = orderForm.customer;
	const response = await fetch("/api/customers", {
		method: "GET",
		headers: { "Accept": "application/json" }
	});
	if (response.ok === true) {
		const customers = await response.json();
		customers["$values"].forEach(product => {
			var option = document.createElement("option");
			option.text = product.name;
			option.value = parseInt(product.id);
			selectList.append(option)
		});
	}
}

async function GetAllWorkers() {
	var selectList = orderForm.worker;
	const response = await fetch("/api/workers", {
		method: "GET",
		headers: { "Accept": "application/json" }
	});
	if (response.ok === true) {
		const workers = await response.json();
		workers["$values"].forEach(product => {
			var option = document.createElement("option");
			option.text = product.name;
			option.value = parseInt(product.id);
			selectList.append(option)
		});
	}
}


async function DeleteOrderById(id) {
	const response = await fetch("/api/orders/" + id, {
		method: "DELETE",
		headers: { "Accept": "application/json" }
	});

	if (response.ok === true) {
		const order = await response.json();
		alert("Успешное удаление");
		document.querySelector("tr[data-rowid='" + order.id + "']").remove();
	}
}

function row(order) {
	const tr = document.createElement("tr");
	tr.setAttribute("data-rowid", order.id);

	const idTd = document.createElement("td");
	idTd.append(order.id);
	tr.append(idTd);

	const CustomerTd = document.createElement("td");
	CustomerTd.append(order.customer.name);
	tr.append(CustomerTd);

	const ProductTd = document.createElement("td");
	ProductTd.append(order.product.name);
	tr.append(ProductTd);

	const WorkerTd = document.createElement("td");
	WorkerTd.append(order.worker.name);
	tr.append(WorkerTd)

	const AmountTd = document.createElement("td");
	AmountTd.append(order.amount);
	tr.append(AmountTd);

	const OrderDateTd = document.createElement("td");
	OrderDateTd.append(order.orderDate);
	tr.append(OrderDateTd);

	const ExecStartDateTd = document.createElement("td");
	ExecStartDateTd.append(order.executionStartDate);
	tr.append(ExecStartDateTd);

	const ImplTd = document.createElement("td");
	ImplTd.append(order.implementationDate);
	tr.append(ImplTd);

	const DeliveryTd = document.createElement("td");
	DeliveryTd.append(order.deliveryOrderDate);
	tr.append(DeliveryTd);

	const linksTd = document.createElement("td");

	const linkForEdit = document.createElement("a");
	linkForEdit.setAttribute("order-id", order.id);
	linkForEdit.append("Edit")
	linkForEdit.addEventListener("click", event => {
		event.preventDefault();
		localStorage.setItem('id', order.id);
		window.location = "edit.html";
	});
	linksTd.append(linkForEdit);

	const linkForDelete = document.createElement("a");
	linkForDelete.setAttribute("order-id", order.id);
	linkForDelete.append("Delete");
	linkForDelete.addEventListener("click", event => {
		event.preventDefault();
		DeleteOrderById(order.id)
	});
	linksTd.append(linkForDelete);

	tr.append(linksTd);
	return tr;
}