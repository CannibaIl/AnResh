<template>
  <section>
    <edit-employee-modal
      ref="editEmployeeModal"
      @row-created="addEmployee"
      @row-updated="replaceEmployee"
    />
    <delete-employee-modal
      ref="deleteEmployeeModal"
      @row-deleted="handleEmployeeDeleted"
    />

    <b-button
      button
      class="btn primary d-block mt-2 mb-4"
      variant="primary"
      v-if="!hasEmployees && !isBusy"
      @click.prevent="handleCreateEmployee()"
    >
      <fa icon="plus" style="transform: scale(0.8)" /> Add
    </b-button>

    <div v-if="isBusy || hasEmployees" class="big-table-wrap table-responsive">
      <div class="mb-2 d-flex flex-wrap justify-content-between">
        <b-button
          button
          @click.prevent="handleCreateEmployee()"
          class="btn primary mr-2 mt-2"
          variant="primary"
          ><fa icon="plus" style="transform: scale(0.8)" /> Add</b-button
        >
        <b-button
          @click.prevent="handleDeleteSelectedEmployees()"
          v-if="selectedEmployees.length"
          class="btn mt-2"
          variant="danger"
          ><fa icon="trash" style="transform: scale(0.8)" /> Delete
          selected</b-button
        >
      </div>

      <b-table
        class="big-table big-table-select-column big-table-buttons"
        striped
        hover
        borderless
        :busy="isBusy"
        :items="employees"
        :fields="fields"
        ref="employeesTable"
        selectable
        @row-selected="onRowSelected"
        thead-class="big-table-head"
        tbody-class="big-table-body"
      >
        <template #head(+)>
          <div
            title="select all"
            class="select-all select-all-false"
            v-if="!selectedEmployees.length"
            @click="selectAllRows"
          >
            <fa icon="check" />
          </div>
          <div
            title="clear selected"
            class="select-all select-all-true"
            v-else
            @click="clearSelected"
          >
            <fa icon="check" />
          </div>
        </template>

        <template #head(*)>
          <span></span>
        </template>

        <template #table-busy>
          <div class="text-center text-danger my-2">
            <b-spinner class="align-middle"></b-spinner>
            <strong>Loading...</strong>
          </div>
        </template>

        <template #cell(*)="row">
          <div class="d-flex table-btn-group">
            <button @click.prevent="handleEditEmployee(row.item)">
              <fa class="mr-2 table-ico table-ico-primary" icon="edit" />
            </button>
            <button @click.prevent="handleDeleteEmployee(row.item)">
              <fa class="table-ico table-ico-danger" icon="trash" />
            </button>
          </div>
        </template>
      </b-table>

      <p class="mt-1" style="color: #fff; margin-left: 44px">1 2 3 ...</p>
    </div>

    <span v-else>There are no employees</span>
  </section>
</template>

<script>
import DeleteEmployeeModal from "~/components/Employee/DeleteEmployeeModal.vue";
import EditEmployeeModal from "~/components/Employee/EditEmployeeModal.vue";
export default {
  components: { EditEmployeeModal, DeleteEmployeeModal },
  props: ["departmentId"],

  data: () => ({
    isBusy: true,
    fields: [
      { key: "+", class: "col-auto text-center" },
      { key: "id", label: "Id" },
      { key: "lastName", label: "LastName" },
      { key: "firstName", label: "FirstName" },
      { key: "middleName", label: "MiddleName" },
      { key: "departmentName", label: "Department" },
      { key: "salary", label: "Salary" },
      { key: "*", class: "col-auto" },
    ],
    selectedEmployees: [],
    employees: [],
  }),

  computed: {
    hasEmployees() {
      return this.employees.length > 0;
    },
  },

  async mounted() {
    const id = this.departmentId;
    let { data } =
      id === ""
        ? await this.$axios.get("/api/employee")
        : await this.$axios.get("/api/employee/department/" + id);
    this.employees = data;
    this.isBusy = false;
  },

  methods: {
    selectAllRows() {
      this.$refs.employeesTable.selectAllRows();
    },

    clearSelected() {
      this.$refs.employeesTable.clearSelected();
    },

    onRowSelected(items) {
      this.selectedEmployees = items;
    },

    handleCreateEmployee() {
      this.$refs["editEmployeeModal"].open();
    },

    handleEditEmployee(employee) {
      this.$refs["editEmployeeModal"].open(employee);
    },

    handleDeleteEmployee(employee) {
      this.$refs["deleteEmployeeModal"].open(employee);
    },

    handleDeleteSelectedEmployees() {
      this.$refs["deleteEmployeeModal"].open(this.selectedEmployees);
    },

    addEmployee(data) {
      this.employees.unshift(data);
    },

    replaceEmployee(data) {
      console.log(data);
      const index = this.employees.indexOf(data);
      this.employees.splice(index, 1, data);
    },

    handleEmployeeDeleted(data) {
      if (Array.isArray(data)) {
        data.forEach((element) => {
          this.employees = this.employees.filter((e) => e !== element);
        });
      } else {
        this.employees = this.employees.filter((e) => e !== data);
      }
    },
  },
};
</script>