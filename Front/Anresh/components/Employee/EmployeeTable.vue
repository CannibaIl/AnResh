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
      class="custom-btn primary d-block mt-2 mb-4"
      variant="primary"
      v-if="!hasEmployees && !isBusy && isAdmin"
      @click.prevent="handleCreateEmployee()"
    >
      <fa icon="plus" style="transform: scale(0.8)" /> Add
    </b-button>

    <div v-if="isBusy || hasEmployees" class="big-table-wrap table-responsive">
      <div class="mb-2 d-flex flex-wrap justify-content-between">
        <b-button
          v-if="isAdmin"
          button
          @click.prevent="handleCreateEmployee()"
          class="custom-btn btn primary mr-2 mt-2"
          variant="primary"
          ><fa icon="plus" style="transform: scale(0.8)" /> Add</b-button
        >
        <b-button
          @click.prevent="handleDeleteSelectedEmployees()"
          v-if="selectedEmployees.length && isAdmin"
          class="custom-btn mt-2"
          variant="danger"
          ><fa icon="trash" style="transform: scale(0.8)" /> Delete
          selected</b-button  
        >
      </div>

      <table 
      :class="isAdmin ? 'table table-hover table-striped table-borderless big-table big-table-buttons big-table-select-column ' 
                      : 'table table-hover table-striped table-borderless big-table'"
      :busy="isBusy"
      >
        <thead>
          <tr>
            <th scope="col" v-if="isAdmin"></th>
            <th scope="col">Id</th>
            <th scope="col">LastName</th>
            <th scope="col">FirstName</th>
            <th scope="col">MiddleName</th>
            <th scope="col">Department</th>
            <th scope="col">Salary</th>
            <th scope="col">Skills</th>
            <th scope="col" v-if="isAdmin"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="employee in employees" :key="employee.id">
            <td v-if="isAdmin">
              <b-form-checkbox
                v-model="selectedEmployees"
                :value="employee.id"
              ></b-form-checkbox>
            </td>
            <td>{{ employee.id }}</td>
            <td>{{ employee.lastName }}</td>
            <td>{{ employee.firstName }}</td>
            <td>{{ employee.middleName }}</td>
            <td>{{ employee.departmentName }}</td>
            <td>{{ employee.salary }} $
            </td>
            <td class="employee-skill-td">
              <span
                class="employee-skill-item"
                v-for="skill in employee.skills"
                :key="skill.id"
              >
                {{ skill.name }}
              </span>
            </td>
            <td v-if="isAdmin">
              <div>
                <button @click.prevent="handleEditEmployee(employee)">
                  <fa class="mr-2 table-ico table-ico-primary" icon="edit" />
                </button>
                <button @click.prevent="handleDeleteEmployee(employee)">
                  <fa class="table-ico table-ico-danger" icon="trash" />
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>

      <div v-if="isBusy" class="text-center text-danger my-2">
        <b-spinner class="align-middle"></b-spinner>
        <strong>Loading...</strong>
      </div>

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
    isAdmin: false,
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
    this.isAdmin = this.$auth.user.role === "Admin";
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
      this.employees.push(data);
    },

    replaceEmployee(data) {
      const index = this.employees.indexOf(data);
      this.employees.splice(index, 1, data);
    },

    handleEmployeeDeleted(data) {
      if (Array.isArray(data)) {
        data.forEach((element) => {
          this.employees = this.employees.filter((e) => e.id !== element);
        });
      } else {
        this.employees = this.employees.filter((e) => e !== data);
      }
    },
  },
};
</script>


<style lang="scss" scoped>
.employee-skill-td {
  .employee-skill-item::after {
    content: ", ";
    position: relative;
    right: 2px;
  }
  .employee-skill-item:last-child::after {
    content: "";
  }
}
</style>