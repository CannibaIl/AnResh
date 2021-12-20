<template>
  <section>
    <edit-employee-modal
      ref="editEmployeeModal"
      @row-updated="updateDataTable"
    />
    <delete-employee-modal
      ref="deleteEmployeeModal"
      @row-updated="updateDataTable"
    />
    <div class="filter-wrap">
    <div class="filter">

        <b-form-group label="First name">
          <b-form-input
            type="text"
            debounce="1000"
            v-model="pageFilter.firstName"
          ></b-form-input>
        </b-form-group>

        <b-form-group label="Min salary">
          <b-form-input
            type="number"
            step="1"
            min="0"
            debounce="1000"
            v-model="pageFilter.minSalary"
          ></b-form-input>
        </b-form-group>

        <b-form-group label="Max salary">
          <b-form-input
            type="number"
            step="1"
            min="0"
            debounce="1000"
            v-model="pageFilter.maxSalary"
          ></b-form-input>
        </b-form-group>

        

        <b-form-group label="Skills:">
          <ul class="select-skill">
            <li v-for="skill in skills" :key="skill.id">
              <b-form-checkbox
                v-model="pageFilter.ListSkillsId"
                :value="skill.id"
                >{{ skill.name }}</b-form-checkbox
              >
            </li>
          </ul>
        </b-form-group>
    </div>
    </div>

    <b-form-group class="department-parents-wrap">
          <label @click="setDepartmentFilter(null, null)">/ departaments</label>
          <label
            class="d-inline mr-1"
            v-for="(departmentParent, index) in departmentParents"
            :key="departmentParent.name"
            @click="setDepartmentFilter(departmentParent, index)"
            >/ {{ departmentParent.name }}
          </label>

          <ul class="select-department">
            <li
              v-for="department in departments"
              :key="department.id"
              @click="setDepartmentFilter(department, null)"
            >
              {{ department.name }}
            </li>
          </ul>
        </b-form-group>

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
        <div class="d-flex">
          <b-button
          v-if="isAdmin"
          button
          @click.prevent="handleCreateEmployee()"
          class="custom-btn btn primary mr-2 mt-2"
          variant="primary"
          ><fa icon="plus" style="transform: scale(0.8)" /> Add</b-button
        >
        <h4 v-show="totalRows !== null" style="color: #fff; margin-top: 13px; margin-left: 10px;">Found {{totalRows}} employees</h4>
        </div>
        

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
        :busy="isBusy"
        :class="
          isAdmin
            ? 'table table-hover table-striped table-borderless big-table big-table-buttons big-table-select-column '
            : 'table table-hover table-striped table-borderless big-table'
        "
      >
        <thead>
          <tr>
            <th class="col" v-if="isAdmin"></th>
            <th class="col-1" @click="orderBy('Id')">Id</th>
            <th class="col-1" @click="orderBy('LastName')">LastName</th>
            <th class="col-1" @click="orderBy('FirstName')">FirstName</th>
            <th class="col-1" @click="orderBy('MiddleName')">MiddleName</th>
            <th class="col-1" @click="orderBy('DepartmentName')">Department</th>
            <th class="col-1" @click="orderBy('Salary')">Salary</th>
            <th class="col-6" @click="orderBy('SkillsCount')">Skills</th>
            <th class="col" v-if="isAdmin"></th>
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
            <td>{{ employee.salary.toFixed(2) }} $</td>
            <td class="employee-skill-td">
              <span
                class="employee-skill-item"
                v-for="(skill, i) in employee.skills"
                :key="i"
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

      <b-pagination
        class="mt-2 custom-pagination"
        v-model="currentPage"
        :total-rows="totalRows"
        :per-page="pageFilter.take"
        @page-click="onPageClick"
      >
      </b-pagination>
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

  data() {
    return {
      currentPage: 1,
      totalRows: null,
      isBusy: true,
      isAdmin: false,
      selectedEmployees: [],
      employees: [],
      skills: [],
      departments: [],
      departmentParents: [],
      pageFilter: {
        take: 10,
        skip: 0,
        orderBy: "FirstName",
        ascDesc: "asc",
        departmentID: null,
        FirstName: "",
        minSalary: null,
        maxSalary: null,
        ListSkillsId: [],
      },
    };
  },

  watch: {
    pageFilter: {
      async handler(value) {
        if (value.maxSalary === "") {
          this.pageFilter.maxSalary = null;
        }
        if (value.minSalary === "") {
          this.pageFilter.minSalary = null;
        }
        this.employees = await this.getEmployees();
      },
      deep: true,
    },
  },

  computed: {
    hasEmployees() {
      return this.employees.length > 0;
    },
  },

  async mounted() {
    this.employees = await this.getEmployees();
    this.isAdmin = this.$auth.user.role === "Admin";
    this.isBusy = false;
    await this.$axios
      .get(`/api/department/childrenAndParents/childId/0`)
      .then((d) => {
        this.departments = d.data.children;
        this.departmentParents = d.data.parents;
      });
    await this.$axios.get("/api/skill").then((d) => {
      this.skills = d.data;
    });
  },

  methods: {
    async getEmployees() {
      this.pageFilter.skip = 0;
      this.pageFilter.take = 10;
      this.currentPage = 1;
      const pageQuery = `?Take=${this.pageFilter.take}
                          &Skip=${this.pageFilter.skip}
                          &OrderBy=${this.pageFilter.orderBy}
                          &AscDesc=${this.pageFilter.ascDesc}`;
      const id = this.departmentId;
      let { data } =
        id === ""
          ? await this.$axios.post("/api/employee/filtred", this.pageFilter)
          : await this.$axios.get("/api/employee/department/" + id + pageQuery);

      this.totalRows = data.total;
      
      return data.employees;
    },

    async setDepartmentFilter(department, index) {
      this.pageFilter.departmentId = department === null ? 0 : department.id;
      let { data } = await this.$axios.get(
        `/api/department/parentId/${this.pageFilter.departmentId}`
      );
      this.employees = await this.getEmployees();

      if (data.length > 0) {
        this.departments = data;
      }

      if (department === null && index === null) {
        this.pageFilter.departmentID = 0;
        this.departmentParents = [];
        return;
      }

      const parentsLength = this.departmentParents.length;
      const lastParent = this.departmentParents[parentsLength - 1];
      if (index !== null) {
        this.departmentParents.splice(index + 1, parentsLength - 1 - index);
      } else {
        if (
          lastParent !== undefined &&
          lastParent.parentId === department.parentId
        ) {
          this.departmentParents.splice(-1);
        }
        this.departmentParents.push(department);
      }
    },

    async onPageClick(event, page) {
      this.pageFilter.skip = (page - 1) * this.pageFilter.take;
    },

    async orderBy(orderBy) {
      if (this.pageFilter.orderBy === orderBy) {
        this.pageFilter.ascDesc =
          this.pageFilter.ascDesc === "asc" ? "desc" : "asc";
      } else {
        this.pageFilter.ascDesc = "asc";
      }
      this.pageFilter.orderBy = orderBy;
      this.pageFilter.skip = 0;
      this.currentPage = 1;
      this.employees = await this.getEmployees();
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

    async updateDataTable() {
      let response = await this.getEmployees();
      if (response.length === 0) {
        this.pageFilter.skip = 0;
        this.currentPage = 1;
        response = await this.getEmployees();
      }
      this.employees = response;
    },
  },
};
</script>


<style lang="scss" scoped>
.employee-skill-td {
  .employee-skill-item::after {
    content: ", ";
    position: relative;
    right: 2px;
  }
  .employee-skill-item:last-child::after {
    content: "";
  }
}
.filter-wrap {
  height: 80px;
  position: relative;
  z-index: 10;
}
.filter{
  position: absolute;
  display: flex;
  width: 100%;
  height: 10px;
  justify-content: space-between;
  .form-group  {
    width: 24%;
    

    .select-skill  {
      padding: 0 ;
      background-color: #fff;
      height: 38px;
      transition: .2s;
      scroll-snap-type: y mandatory;
      li{   
        list-style: none;
        scroll-snap-align: start;
        div{
          height: 38px;
          padding-top: 5px;
          margin-left: 10px;
          input {
            margin-left: 10px;
          }
        }
      }
      &:hover {
        height: calc(38px * 4)
      }
    }
  }
}
.select-department  {
      background: #fff!important;
      height: calc(38px * 4);
      scroll-snap-type: y mandatory;
      li {
        line-height: 38px;
        scroll-snap-align: start;
      }
    }
</style>